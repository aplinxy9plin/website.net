﻿using ActiveCitizen.Model.StaticContent.FAQ;
using ActiveCitizenWeb.DataAccess.Provider;
using ActiveCitizenWeb.StaticContentCMS.Controllers;
using ActiveCitizenWeb.StaticContentCMS.ViewModel.FAQ;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ActiveCitizenWeb.Tests.Controllers
{
    //push travis
    [TestClass]
    public class FaqControllerTests
    {
        private FAQController faqController;

        [TestMethod]
        public void IndexFaqListItems()
        {
            var category1 = new FaqListCategory { Id = 1, Order = 1 };
            var category2 = new FaqListCategory { Id = 2, Order = 2 };
            var category3 = new FaqListCategory { Id = 3 };
            var category4 = new FaqListCategory { Id = 4 };

            var item1 = new FaqListItem { Id = 1, Order = 1, Category = category2 };
            var item2 = new FaqListItem { Id = 2, Order = 2, Category = category1 };
            var item3 = new FaqListItem { Id = 3, Order = 1, Category = category1 };

            var items = new List<FaqListItem> { item1, item2, item3 };
            var categories = new List<FaqListCategory> { category1, category2, category3, category4 };

            categories.ForEach(cat => cat.Items = items.Where(i => i.Category == cat).ToList());
            category4.Items = null;


            var providerMock = new Mock<IStaticContentProvider>();
            providerMock.Setup(p => p.GetAllItems()).Returns(items);
            providerMock.Setup(p => p.GetAllCategories()).Returns(categories);

            faqController = new FAQController(providerMock.Object, null);

            var action = faqController.Index();

            Assert.IsInstanceOfType(action, typeof(ViewResult));
            var view = (ViewResult)action;

            Assert.IsInstanceOfType(view.Model, typeof(QuestionsVM));

            var model = (QuestionsVM)view.Model;

            // Number of elements
            Assert.AreEqual(3, model.Questions.Count);
            // Sort order
            Assert.AreEqual(0, model.Questions.IndexOf(item3));
            Assert.AreEqual(1, model.Questions.IndexOf(item2));
            Assert.AreEqual(2, model.Questions.IndexOf(item1));

            //Categories with no items
            Assert.IsTrue(
                model.Categories.Count == 2 &&
                model.Categories.IndexOf(category3) >= 0 &&
                model.Categories.IndexOf(category4) >= 0,
                "Categories list must contain only categories with empty Items list"
            );
        }

        [TestMethod]
        public void EditCategory()
        {
            var cat = new FaqListCategory();

            var provider = new Mock<IStaticContentProvider>();
            provider.Setup(p => p.GetCategory(It.IsAny<int>())).Returns(cat);


            faqController = new FAQController(provider.Object, null);
            var action = faqController.EditCategory(0);

            Assert.IsInstanceOfType(action, typeof(ViewResult));
            var view = (ViewResult)action;

            Assert.IsInstanceOfType(view.Model, typeof(FaqListCategory));
            var model = (FaqListCategory)view.Model;
            Assert.AreSame(cat, model);
        }

        [TestMethod]
        public void EditQuestion()
        {
            FaqListItem item = new FaqListItem();
            QuestionVM vm = new QuestionVM();
            List<FaqListCategory> list = new List<FaqListCategory>() { new FaqListCategory() };

            var cont = new Mock<IStaticContentProvider>();
            cont.Setup(p => p.GetFaqListItem(It.IsAny<int>())).Returns(item);
            cont.Setup(p => p.GetAllCategories()).Returns(list);

            var map = new Mock<IMapper>();
            map.Setup(m => m.Map<QuestionVM>(It.IsAny<FaqListItem>())).Returns(vm);

            faqController = new FAQController(cont.Object, map.Object);
            var action = faqController.EditQuestion(0);

            map.Verify(m => m.Map<QuestionVM>(item), Times.AtLeastOnce, "Does not mapped model to view model");
            Assert.IsInstanceOfType(action, typeof(ViewResult));
            var model = ((ViewResult)action).Model;

            Assert.AreSame(vm, model);
        }

        [TestMethod]
        public void NewQuestion()
        {
            List<FaqListCategory> list = new List<FaqListCategory>
            {
                new FaqListCategory
                {
                    Items = new List<FaqListItem>
                    {
                        new FaqListItem() { Order = 10 }
                    }
                }
            };

            var cont = new Mock<IStaticContentProvider>();
            cont.Setup(p => p.GetAllCategories()).Returns(list);
            cont.Setup(p => p.GetCategory(It.IsAny<int>())).Returns(list[0]);

            faqController = new FAQController(cont.Object, null);
            var action = faqController.NewQuestion();
            Assert.IsInstanceOfType(action, typeof(ViewResult));

            var view = (ViewResult)action;
            Assert.AreEqual("EditQuestion", view.ViewName);
            Assert.IsInstanceOfType(view.Model, typeof(QuestionVM));

            var model = (QuestionVM)view.Model;
            Assert.AreEqual(20, model.Order);
        }

        [TestMethod]
        public void NewCategory()
        {
            var provider = new Mock<IStaticContentProvider>();
            provider.Setup(p => p.GetAllCategories()).Returns(new List<FaqListCategory>
            {
                new FaqListCategory() { Order = 10 }
            });

            faqController = new FAQController(provider.Object, null);
            var view = faqController.NewCategory() as ViewResult;

            Assert.AreEqual(11, ((FaqListCategory)view.Model).Order);
        }

        [TestMethod]
        public void SaveQuestion()
        {
            var provider = new Mock<IStaticContentProvider>();
            var mapper = new MapperMock();

            faqController = new FAQController(provider.Object, mapper);
        }
    }
}
