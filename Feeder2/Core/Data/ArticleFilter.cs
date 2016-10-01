using Feeder.Common.Factory;
using Feeder.Common.Model;
using Feeder.Common.Repository;
using Feeder.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.Common.Data
{
    public class ArticleFilter
    {
        public List<FeedItemViewModel> GetAllArticles()
        {
            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            if (repo.FeedGroups == null)
            {
                return new List<FeedItemViewModel>();
            }

            var models = from grp in repo.FeedGroups
                         from feed in grp.Feeds
                         from article in feed.Items
                         orderby article.PublishDate descending
                         select article;
            var viewModels = new List<FeedItemViewModel>();
            foreach (var model in models)
            {
                viewModels.Add(FeedItemViewModel.Create(model));
            }

            return viewModels;
        }

        public List<FeedItemViewModel> GetAllArticles(FeedGroupModel feedGroup)
        {
            var models = from feed in feedGroup.Feeds
                         from article in feed.Items
                         orderby article.PublishDate descending
                         select article;
            var viewModels = new List<FeedItemViewModel>();
            foreach (var model in models)
            {
                viewModels.Add(FeedItemViewModel.Create(model));
            }

            return viewModels;
        }

        public List<FeedItemViewModel> GetUnreadArticles()
        {
            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            if (repo.FeedGroups == null)
            {
                return new List<FeedItemViewModel>();
            }

            var models = from grp in repo.FeedGroups
                         from feed in grp.Feeds
                         from article in feed.Items
                         where !article.IsRead
                         orderby article.PublishDate descending
                         select article;
            var viewModels = new List<FeedItemViewModel>();
            foreach (var model in models)
            {
                viewModels.Add(FeedItemViewModel.Create(model));
            }

            return viewModels;
        }

        public List<FeedItemViewModel> GetUnreadArticles(FeedGroupModel feedGroup)
        {
            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            if (repo.FeedGroups == null)
            {
                return new List<FeedItemViewModel>();
            }

            var models = from feed in feedGroup.Feeds
                         from article in feed.Items
                         where !article.IsRead
                         orderby article.PublishDate descending
                         select article;
            var viewModels = new List<FeedItemViewModel>();
            foreach (var model in models)
            {
                viewModels.Add(FeedItemViewModel.Create(model));
            }

            return viewModels;
        }

        public async Task<List<FeedItemViewModel>> GetSavedArticles()
        {
            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            var results = await repo.LoadSavedArticlesFromStorage();
            var vms = results.Select(model => FeedItemViewModel.Create(model));
            return vms.ToList();
        }

        public async Task<List<FeedItemViewModel>> GetSavedArticles(FeedGroupModel group)
        {
            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            var results = await repo.LoadSavedArticlesFromStorage();
            var vms = results
                .Where(article => repo.ContainsArticle(group.Title, article.Id))
                .Select(model => FeedItemViewModel.Create(model));
            return vms.ToList();
        }
    }
}
