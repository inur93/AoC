using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2024;

internal class Day09
{
    [Test]
    [Arguments("09", 1)]
    [Arguments("09S", 2)]
    public async Task Puzzle1(string input, int result)
    {
        var diskMap = await FileParser.GetText(input);



        await Assert.That(data.Length).IsEqualTo(result);
    }
}
