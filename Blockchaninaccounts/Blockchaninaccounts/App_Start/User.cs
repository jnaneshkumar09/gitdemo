using System;
using System.Collections.Generic;

namespace Blockchaninaccounts
{
    public class User
    {

    }
    public class UserPhase
    {
        public static List<string> SecretPhase()
        {
            int count = 1;
            List<string> responses = new List<String>();
            responses.AddRange(new String[] {"Hello","HI","Random","One","MonKey","Window","Door", "Icecreem","suger","razor","Jam","Zim","part","today","time","title",
            "Ticket","tata","tea","milk","Mango","Delhi","Bottom","tom","dom","Phone","Tab","Bell"});
            Random random = new Random();

            List<String> responses1 = new List<String>();
            while (count != 26)
            {
                string res = responses[random.Next(0, responses.Count)];
                responses1.Add(res);
                // responses1.AddRange(new String[] { res });
                count++;
            }
            return responses1;
        }
    }
}