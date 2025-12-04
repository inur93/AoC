using System.Text;
using AoC2024.Models;

namespace AoC2024;

internal class Day09
{
    [Test]
    [Arguments("09", 6360094256423)]
    [Arguments("09S", 1928)]
    public async Task Puzzle1(string input, long result)
    {
        var diskMap = (await FileParser.GetText(input)).ToCharArray();
        var filesystem = GetFilesystem(diskMap);
        var blocks = GetBlocks(filesystem).ToList();
        var files = blocks.Where(x => x.Id != -1).Reverse().ToList();

        foreach (var file in files)
        {
            while (file.Length > 0)
            {
                var freeBlock = blocks.FirstOrDefault(x => x.IsFree && x.Length > 0 && x.End < file.Start);
                if (freeBlock == null)
                {
                    break;
                }
                var length = file.Length > freeBlock.Length ? freeBlock.Length : file.Length;

                for (var i = freeBlock.Start; i < freeBlock.Start + length; i++)
                {
                    filesystem[i] = file.Id.ToString();
                }
                for (var i = file.End; i > file.End - length; i--)
                {
                    filesystem[i] = ".";
                }

                file.End -= length;
                freeBlock.Start += length;
            }
        }

        var checksum = GetChecksum(filesystem);

        await Assert.That(checksum).IsEqualTo(result);
    }



    [Test]
    [Arguments("09", 6379677752410)]
    [Arguments("09S", 2858)]
    public async Task Puzzle2(string input, long result)
    {
        var diskMap = (await FileParser.GetText(input)).ToCharArray();

        var filesystem = GetFilesystem(diskMap);
        var blocks = GetBlocks(filesystem).ToList();
        var files = blocks.Where(x => x.Id != -1).Reverse().ToList();

        foreach (var file in files)
        {
            var freeBlock = blocks.FirstOrDefault(x => x.IsFree && x.Length >= file.Length && x.End < file.Start);
            if (freeBlock == null)
            {
                continue;
            }

            for (var i = file.Start; i <= file.End; i++)
            {
                filesystem[i] = ".";
            }

            for (var i = freeBlock.Start; i < freeBlock.Start + file.Length; i++)
            {
                filesystem[i] = file.Id.ToString();
            }
            file.Id = -1;
            freeBlock.Start += file.Length;
        }
        var sum = GetChecksum(filesystem);
        await Assert.That(sum).IsEqualTo(result);
    }

    private IEnumerable<Block> GetBlocks(List<string> filesystem)
    {
        var position = 0;
        var id = filesystem[position++];
        var start = 0;
        while (position < filesystem.Count)
        {
            var value = filesystem[position];
            if (value == id)
            {
                position++;
                continue;
            }
            if (value != id)
            {
                yield return new Block
                {
                    Id = id == "." ? -1 : int.Parse(id),
                    Start = start,
                    End = position - 1,
                };
                id = value;
                start = position;
            }
            position++;
        }
        yield return new Block
        {
            Id = id == "." ? -1 : int.Parse(id),
            Start = start,
            End = position - 1,
        };
    }

    private List<string> GetFilesystem(char[] diskMap)
    {
        var id = 0;
        var filesystem = new List<string>();
        for (var i = 0; i < diskMap.Length; i++)
        {
            var _id = i % 2 == 0 ? id++ : -1;
            for (var j = 0; j < int.Parse(diskMap[i].ToString()); j++)
            {
                filesystem.Add(_id == -1 ? "." : $"{_id}");
            }
        }

        return filesystem;
    }

    private long GetChecksum(List<string> filesystem)
    {
        long sum = 0;
        for (var i = 0; i < filesystem.Count; i++)
        {
            var value = filesystem[i];
            if (value != ".")
            {
                sum += int.Parse(value) * i;
            }

        }

        return sum;
    }
}
