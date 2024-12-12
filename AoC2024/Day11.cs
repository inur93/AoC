namespace AoC2024;

internal class Day11
{
    [Test]
    [Arguments("11S", 7, 1)]
    [Arguments("11S1", 22, 6)]
    [Arguments("11", 199986, 25)]
    //[Arguments("11", 199986, 75)]
    public async Task Puzzle1(string input, int result, int numberOfBlinks)
    {
        var values = (await FileParser.GetNumbers(input, " ")).First();
        var first = new Stone { Value = values[0] };
        var previous = first;
        for (var i = 1; i < values.Length; i++)
        {
            var stone = new Stone
            {
                Value = values[i],
                Left = previous,
            };

            previous.Right = stone;
            previous = stone;
        }

        for (var i = 0; i < numberOfBlinks; i++)
        {
            var stone = first;

            while (stone != null)
            {

                if (stone.Value == 0)
                {
                    stone.Value = 1;
                    stone = stone.Right;
                    continue;
                }

                var strValue = stone.Value.ToString();
                if (strValue.Length % 2 == 0)
                {
                    var left = new Stone
                    {
                        Value = int.Parse(strValue.Substring(0, strValue.Length / 2)),
                        Left = stone.Left,
                    };
                    var right = new Stone
                    {
                        Value = int.Parse(strValue.Substring(strValue.Length / 2)),
                        Right = stone.Right,
                        Left = left
                    };
                    left.Right = right;

                    if (stone.Left != null)
                        stone.Left.Right = left;
                    if (stone.Right != null)
                        stone.Right.Left = right;

                    if(stone == first)
                        first = left;

                    stone = right.Right;
                    continue;
                }

                stone.Value = stone.Value * 2024;
                stone = stone.Right;
            }

        }

        var list = ToList(first);
        var count = list.Count;
        await Assert.That(count).IsEqualTo(result);
    }

    private List<Stone> ToList(Stone stone)
    {
        var list = new List<Stone>();
        while (stone != null)
        {
            list.Add(stone);
            stone = stone.Right;
        }

        return list;
    }


}


class Stone
{
    public long Value { get; set; }
    public Stone? Left { get; set; }
    public Stone? Right { get; set; }
}
