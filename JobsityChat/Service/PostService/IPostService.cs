using Model;
using System.Collections.Generic;

namespace Service.PostService
{
    public interface IPostService
    {
        Post Add(Post post);

        IList<Post> List(int page = 1, int pageSize = 50);
    }
}
