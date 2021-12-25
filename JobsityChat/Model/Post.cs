using System;

namespace Model
{
    public class Post
    {
        public virtual int Id { get; set; }

        public virtual string User { get; set; }

        public virtual string Message { get; set; }

        public virtual DateTime Date { get; set; }
    }
}
