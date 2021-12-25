using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class User
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual string Username { get; set; }

        [Required]
        public virtual string Password { get; set; }
    }
}
