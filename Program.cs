using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                int choice = 0;
                do
                {


                    Console.WriteLine("1) Display all Blogs");
                    Console.WriteLine("2) Add Blog");
                    Console.WriteLine("3) Create post");

                    if (choice == 2)
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }

                    else if (choice == 1)
                    {
                        var db = new BloggingContext();
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }

                    else if (choice == 3)
                    {
                        var db = new BloggingContext();

                        Console.WriteLine("Create post\n");

                        var query = db.Blogs.OrderBy(b => b.Name);
                        
                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            
                            Console.WriteLine($"{item.BlogId} + {item.Name}");
                        }

                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            if (db.Blogs.Any(b=> b.BlogId == BlogId))
                            {
                                Post post = new Post();
                                post.BlogId = BlogId;
                                Console.WriteLine("Enter the Title");
                                post.Title = Console.ReadLine();
                                if (post.Title.Length == 0)
                                {
                                    logger.Error("Titles cant be blank");
                                }
                                else
                                {
                                    Console.WriteLine("Enter post info");
                                    post.Content = Console.ReadLine();
                                    db.AddPost(post);
                                    logger.Info("{title} added.", post.Title);

                                }

                            }else
                            {
                                logger.Error("There are no Blogs with that id");
                            }
                        }
                        else
                        {
                            logger.Error("No Blog with that ID");
                        }

                    }
                    else if (choice == 4)
                    {
                        // Display Posts
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("Select the blog's posts to display:");
                        Console.WriteLine("0) Posts from all blogs");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}) Posts from {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            IEnumerable<Post> Posts;
                            if (BlogId != 0 && db.Blogs.Count(b => b.BlogId == BlogId) == 0)
                            {
                                logger.Error("There are no Blogs saved with that Id");
                            }
                            else
                            {
                                // display posts from all blogs
                                Posts = db.Posts.OrderBy(p => p.Title);
                                if (BlogId == 0)
                                {
                                    // display all posts from all blogs
                                    Posts = db.Posts.OrderBy(p => p.Title);
                                }
                                else
                                {
                                    // display post from selected blog
                                    Posts = db.Posts.Where(p => p.BlogId == BlogId).OrderBy(p => p.Title);
                                }
                                Console.WriteLine($"{Posts.Count()} post(s) returned");
                                foreach (var item in Posts)
                                {
                                    Console.WriteLine($"Blog: {item.Blog.Name}\nTitle: {item.Title}\nContent: {item.Content}\n");
                                }
                            }
                        }
                        else
                        {
                            logger.Error("Invalid Blog Id");
                        }
                    }
                    Console.WriteLine();

                } while (choice == 5);
                }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
