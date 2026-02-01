using DSA_mid_project.BL;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DSA_mid_project
{
    public class PostsCrud
    {
        public PostQueue postQueue = new PostQueue();
        private CommentQueue commentQueue = new CommentQueue();
        private List<Likes> likes;


        public PostsCrud()
        {
            LoadPostsFromDatabase(); 
            LoadCommentsFromDatabase();
            likes = new List<Likes>();
            LoadAllLikes();
        }

        public void LoadPostsFromDatabase()
        {
            string query = "SELECT p.*, COUNT(c.comment_id) as comment_count FROM posts p LEFT JOIN comments c ON p.post_id = c.post_id GROUP BY p.post_id ORDER BY p.created_at";
            var reader = DatabaseHelper.Instance.getData(query);

            while (reader.Read())
            {
                int postId = Convert.ToInt32(reader["post_id"]);

                // ID =0, not loaded
                if (postId <= 0)
                {
                    continue;
                }

                Post post = new Post
                {
                    PostId = postId,
                    UserId = Convert.ToInt32(reader["user_id"]),
                    Content = reader["content"].ToString(),
                    Likes = Convert.ToInt32(reader["likes_count"]),
                    CommentsCount = Convert.ToInt32(reader["comment_count"]),
                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                };
                postQueue.EnqueuePost(post);
            }
            reader.Close();
        }

        public DataTable GetAllPosts()
        {
            DataTable dt = CreatePostsDataTable();
            List<Post> posts = postQueue.GetAllPosts();

            foreach (Post post in posts)
            {
                if (post.PostId <= 0)
                {
                    continue; 
                }

                Users user = LaunchPage.userCrud.ViewProfile(post.UserId);

                if (user == null)
                {
                    MessageBox.Show($"User ID {post.UserId} not found in userList!");
                }
                else
                {
                    dt.Rows.Add(post.PostId, user.Username, post.Content, post.Likes, post.CommentsCount, post.CreatedAt);
                }
            }

            return dt;
        }

        public bool CreatePost(string content)
        {
          
            try
            {
                string query = $"INSERT INTO posts (user_id, content, likes_count, comment_count) VALUES ({SessionManager.UserID}, '{content.Replace("'", "''")}', 0, 0)";

                int rowsAffected = DatabaseHelper.Instance.Update(query);
                if (rowsAffected > 0)
                {                                                                                                    
                 
                    LoadPostsFromDatabase(); //to reload
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating post: {ex.Message}");
                return false;
            }
        }


        public bool LikePost(int postId)
        {

            Post post = postQueue.FindPost(postId);
            if (post != null)
            {
                string query = $"INSERT INTO likes (post_id, user_id) VALUES ({postId}, {SessionManager.UserID})";
                if (DatabaseHelper.Instance.Update(query) > 0)
                {

                    string updatePostQuery = $"UPDATE posts SET likes_count = likes_count + 1 WHERE post_id = {postId}";
                    DatabaseHelper.Instance.Update(updatePostQuery);

                    post.Likes++;
                    likes.Add(new Likes { Post_id = postId, User_id = SessionManager.UserID });
                    return true;
                }
            }
            return false;
        }

        public bool UnlikePost(int postId)
        {

            bool found = false;
            foreach (Likes like in likes)
            {
                if (like.Post_id == postId && like.User_id == SessionManager.UserID)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                MessageBox.Show("You haven't liked this post yet!");
                return false;
            }

            Post post = postQueue.FindPost(postId);

            if (post != null)
            {
                string query = $"DELETE FROM likes WHERE post_id = {postId} AND user_id = {SessionManager.UserID}";


                if (DatabaseHelper.Instance.Update(query) > 0)
                {
                    // in table
                    string updatePostQuery = $"UPDATE posts SET likes_count = likes_count - 1 WHERE post_id = {postId}";
                    DatabaseHelper.Instance.Update(updatePostQuery);

                    post.Likes--;
                    // from list
                    return true;
                }
            }
            return false;
        }

        public bool HasUserLikedPost(int postId)
        {
            foreach (Likes like in likes)
            {
                if (like.Post_id == postId && like.User_id == SessionManager.UserID)
                    return true;
            }
            return false;
        }

        private void LoadAllLikes()
        {
            string query = "SELECT * FROM likes";
            MySqlDataReader reader = DatabaseHelper.Instance.getData(query);

            DataTable dt = new DataTable();
            dt.Load(reader); 

            foreach (DataRow row in dt.Rows)
            {
                likes.Add(new Likes
                {
                    Post_id = Convert.ToInt32(row["post_id"]),
                    User_id = Convert.ToInt32(row["user_id"])
                });
            }
            reader.Close(); 
        }
        public bool DeletePost(int postId)
        {
            if (postQueue.DeletePost(postId))
            {
                string query = $"DELETE FROM posts WHERE post_id = {postId}";
                return DatabaseHelper.Instance.Update(query) > 0;
            }
            return false;
        }

        public bool RestorePost(int postID, string content)
        {
            string query = $"INSERT INTO Posts (post_id,user_id,content,likes_count,comment_count) VALUES ('{postID}', '{SessionManager.UserID}', '{content.Replace("'", "''")}', 0,0)";
            int rows = DatabaseHelper.Instance.Update(query);

            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        public bool EditPost(int postId, string newContent)
        {
            if (postQueue.EditPost(postId, newContent))
            {
                string query = $"UPDATE posts SET content = '{newContent}' WHERE post_id = {postId}";
                return DatabaseHelper.Instance.Update(query) > 0;
            }
            return false;
        }

        public DataTable GetUserPosts()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PostID", typeof(int));
            dt.Columns.Add("Content", typeof(string));
            dt.Columns.Add("Likes", typeof(int));
            dt.Columns.Add("Comments", typeof(int));
            dt.Columns.Add("CreatedAt", typeof(DateTime));

            List<Post> posts = postQueue.GetAllPosts();
            foreach (Post post in posts)
            {
                if (post.UserId == SessionManager.UserID) 
                {
                    dt.Rows.Add(post.PostId,post.Content, post.Likes, post.CommentsCount ,post.CreatedAt);
                }
            }
            return dt;
        }

        private void LoadCommentsFromDatabase()
        {
            string query = "SELECT * FROM comments";
            var reader = DatabaseHelper.Instance.getData(query);

            while (reader.Read())
            {
                Comments comment = new Comments
                {
                    CommentId = Convert.ToInt32(reader["comment_id"]),
                    PostId = Convert.ToInt32(reader["post_id"]),
                    UserId = Convert.ToInt32(reader["user_id"]),
                    Content = reader["content"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                };
                commentQueue.EnqueueComment(comment);
            }
            reader.Close();
        }

        public bool AddComment(int postId, string content)
        {
            string query = $"INSERT INTO comments (post_id, user_id, content) VALUES ({postId}, {SessionManager.UserID}, '{content}')";
            if (DatabaseHelper.Instance.Update(query) > 0)
            {
                Post post = postQueue.FindPost(postId);
                if (post != null)
                {
                    string updatePostQuery = $"UPDATE posts SET comment_count = comment_count + 1 WHERE post_id = {postId}";
                    DatabaseHelper.Instance.Update(updatePostQuery);

                    if (post != null)
                    {
                        post.CommentsCount++;
                    }
                }

                string getCommentId = "SELECT LAST_INSERT_ID() as comment_id";
                var reader = DatabaseHelper.Instance.getData(getCommentId);
                if (reader.Read())
                {
                    Comments comment = new Comments
                    {
                        CommentId = Convert.ToInt32(reader["comment_id"]),
                        PostId = postId,
                        UserId = SessionManager.UserID,
                        Content = content,
                        CreatedAt = DateTime.Now
                    };
                    commentQueue.EnqueueComment(comment);
                }
                reader.Close();
                return true;
            }
            return false;
        }

        //bubble sort
        private List<Post> SortByLikes(List<Post> posts)
        {
            List<Post> sortedPosts = new List<Post>(posts);

            for (int i = 0; i < sortedPosts.Count - 1; i++)
            {
                for (int j = 0; j < sortedPosts.Count - i - 1; j++)
                {
                    if (sortedPosts[j].Likes < sortedPosts[j + 1].Likes)
                    {
                        Post temp = sortedPosts[j];
                        sortedPosts[j] = sortedPosts[j + 1];
                        sortedPosts[j + 1] = temp;
                    }
                }
            }
            return sortedPosts;
        }

        // Linear Search
        private List<Post> SearchByKeyword(List<Post> posts, string keyword)
        {
            List<Post> results = new List<Post>();

            foreach (Post post in posts)
            {
                if (post.Content.ToLower().Contains(keyword.ToLower()))
                {
                    results.Add(post);
                }
            }
            return results;
        }

        
        public DataTable GetTrendingPosts()
        {
            DataTable dt = CreatePostsDataTable();
            List<Post> posts = postQueue.GetAllPosts();
            List<Post> sortedPosts = SortByLikes(posts);

            foreach (Post post in sortedPosts)
            {
                Users user = LaunchPage.userCrud.ViewProfile(post.UserId);
                if (user != null)
                {
                    dt.Rows.Add(post.PostId,user.Username, post.Content, post.Likes, post.CommentsCount, post.CreatedAt);
                }
            }
            return dt;
        }

        public DataTable SearchPosts(string keyword)
        {
            DataTable dt = CreatePostsDataTable();
            List<Post> posts = postQueue.GetAllPosts();
            List<Post> results = SearchByKeyword(posts, keyword);

            foreach (Post post in results)
            {
                Users user = LaunchPage.userCrud.ViewProfile(post.UserId);
                if (user != null)
                {
                    dt.Rows.Add(post.PostId,user.Username, post.Content, post.Likes, post.CommentsCount, post.CreatedAt);
                }
            }
            return dt;
        }

        // Selection Sort
        private List<Post> SortByNewest(List<Post> posts)
        {
            List<Post> sortedPosts = new List<Post>(posts);

            for (int i = 0; i < sortedPosts.Count - 1; i++)
            {
                int maxIndex = i;
                for (int j = i + 1; j < sortedPosts.Count; j++)
                {
                    if (sortedPosts[j].CreatedAt > sortedPosts[maxIndex].CreatedAt)
                    {
                        maxIndex = j;
                    }
                }

                Post temp = sortedPosts[i];
                sortedPosts[i] = sortedPosts[maxIndex];
                sortedPosts[maxIndex] = temp;
            }
            return sortedPosts;
        }

        public DataTable GetRecentPosts()
        {
            DataTable dt = CreatePostsDataTable();
            List<Post> posts = postQueue.GetAllPosts();
            List<Post> sortedPosts = SortByNewest(posts);

            foreach (Post post in sortedPosts)
            {
                Users user = LaunchPage.userCrud.ViewProfile(post.UserId);
                if (user != null)
                {
                    dt.Rows.Add(post.PostId, user.Username, post.Content, post.Likes, post.CommentsCount, post.CreatedAt);
                }
            }
            return dt;
        }

        private DataTable CreatePostsDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Sr.no" , typeof(int));
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("Content", typeof(string));
            dt.Columns.Add("Likes", typeof(int));
            dt.Columns.Add("Comments", typeof(int));
            dt.Columns.Add("CreatedAt", typeof(DateTime));
            return dt;
        }


        
    }
}


