using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    public class TwineRoot
    {
        public List<Passage> passages;
    }

    public class Passage
    {
        public int pid;
        public string name;
        public string text;
        public List<PassageLink> links;
        public List<string> tags;
    }

    public class PassageLink
    {
        public int connectedToPid;
    }
}