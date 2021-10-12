﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Helpers.Models.DtoModels.MainDbDto
{
    [Serializable]
    public partial class JobPostCommentDto
    {
        public int JobPostCommentID { get; set; }
        public int JobID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int SubCommentID { get; set; }
    }
}
