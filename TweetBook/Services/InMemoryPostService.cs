﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class InMemoryPostService : IPostService
    {
        private readonly List<Post> posts;

        public InMemoryPostService()
        {
            this.posts = new List<Post>();
        }
        
        public List<Post> GetPosts()
        {
            return this.posts;
        }

        public Option<Post> GetPostById(Guid postId)
        {
            return this.posts.SingleOrDefault(x => x.Id == postId);
        }

        public Post Create(string name, List<string> tags, string userId)
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Name = name,
                UserId = userId,
                Tags = tags
            };
            
            this.posts.Add(post);

            return post;
        }

        public bool UpdatePost(Post postToUpdate)
        {
            var postOption = GetPostById(postToUpdate.Id);

            return postOption.Match(_ =>
                {
                    var index = this.posts.FindIndex(x => x.Id == postToUpdate.Id);
                    this.posts[index] = postToUpdate;
                    return true;
                },
                () => false);
        }

        public bool DeletePost(Guid postId)
        {
            var postOption = GetPostById(postId);

            return postOption.Match(post =>
                {
                    this.posts.Remove(post);
                    return true;
                },
                () => false);
        }

        public bool UserOwnsPost(Guid postId, string userId)
        {
            var post = this.posts.SingleOrDefault(x => x.Id == postId);

            return post != null && post.UserId == userId;
        }

        public List<string> GetAllTags()
        {
            List<string> tags = new List<string>();
            foreach (var post in this.posts.Where(x => x.Tags != null))
            {
                tags.AddRange(post.Tags);
            }
            
            return tags;
        }

        public bool DeleteTag(string tagName)
        {
            bool found = false;
            foreach (var post in this.posts.Where(x => x.Tags.Contains(tagName)))
            {
                post.Tags.Remove(tagName);
                found = true;
            }

            return found;
        }
    }
}