
namespace AoC2024;

public partial class Day05
{
    [Test]
    [Arguments("05AS", "05BS", 143)]
    [Arguments("05A", "05B", 5064)]

    public async Task Puzzle1(string ruleInput, string orderInput, int expectedResult)
    {
        var rules = (await FileParser.GetNumbers(ruleInput, "|")).ToArray();
        var ordering = (await FileParser.GetNumbers(orderInput, ",")).ToArray();

        var sum = 0;
        foreach (var order in ordering)
        {
            if (IsOrderValid(order, rules))
            {
                var middleNumber = order[(order.Length / 2)];
                sum += middleNumber;
            }

        }

        await Assert.That(sum).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("05AS", "05BS", 123)]
    [Arguments("05A", "05B", 5152)]

    public async Task Puzzle2(string ruleInput, string orderInput, int expectedResult)
    {
        var rules = (await FileParser.GetNumbers(ruleInput, "|")).ToArray();
        var ordering = (await FileParser.GetNumbers(orderInput, ",")).ToArray();

        var sum = 0;
        foreach (var order in ordering)
        {
            if (!IsOrderValid(order, rules))
            {
                var newOrder = ReOrder(order, rules);
                var middleNumber = newOrder[(newOrder.Length / 2)];
                sum += middleNumber;
            }

        }

        await Assert.That(sum).IsEqualTo(expectedResult);
    }

    private int[] ReOrder(int[] order, int[][] rules)
    {
        var newOrder = new int[order.Length];
        for (var i = 0; i < order.Length; i++)
        {
            var a = order[i];

            while (true)
            {
                var isValid = true;
                for (var j = i + 1; j < order.Length; j++)
                {
                    var b = order[j];
                    var rule = rules.FirstOrDefault(x => x.Contains(a) && x.Contains(b));

                    if (!(rule == null || rule[0] == a))
                    {
                        isValid = false;
                        order[j] = a;
                        a = b;
                        break;
                    }
                }

                if (isValid)
                {
                    newOrder[i] = a;
                    break;
                }
            }
        }

        //sanity check
        if (!IsOrderValid(newOrder, rules))
        {
            throw new Exception("Invalid order");
        }


        return newOrder;
    }

    private static bool IsOrderValid(int[] order, IEnumerable<int[]> rules)
    {
        for (var i = 0; i < order.Length; i++)
        {
            var a = order[i];
            for (var j = i + 1; j < order.Length; j++)
            {
                var b = order[j];
                var rule = rules.FirstOrDefault(x => x.Contains(a) && x.Contains(b));

                if (!(rule == null || rule[0] == a))
                {
                    return false;
                }
            }
        }

        return true;
    }

}
