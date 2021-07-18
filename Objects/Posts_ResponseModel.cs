using System;
using System.Collections.Generic;
using System.Text;

namespace FIS.Objects
{
    public class Posts_ResponseModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public long UserId { get; set; }
    }
}
