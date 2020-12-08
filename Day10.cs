using System.Collections.Generic;

namespace ConsoleApplication5
{
    class Day10
    {
        public void Calculate()
        {
            var input = new List<int> { 3,1,1,3,3,2,2,1,1,3 };
            for (int i = 0; i < 50; i++)
            {
                input = Say(input);
            }
        }


        public List<int> Say(List<int> input)
        {
            var output = new List<int>();

            for (int i = 0; i < input.Count;)
            {
                int num = 1;
                for (int j = i + 1; j < input.Count && input[i] == input[j]; j++)
                    num++;

                output.Add(num);
                output.Add(input[i]);

                i = i + num;
            }

            return output;
        }
    }
}