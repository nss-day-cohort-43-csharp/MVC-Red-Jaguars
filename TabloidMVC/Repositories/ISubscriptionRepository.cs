using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ISubscriptionRepository
    {
        public void Add(Subscription subscription);
        public List<Subscription> GetUserSubscriptions(int id);

        public Subscription GetSubscriptionById(int id);

        public void Edit(Subscription subscription);
    }
}
