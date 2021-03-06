using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB.Wrapper.Interface;
using Xunit;

namespace LiteDB.Wrapper.Test
{
    public class WrapperTest
    {
        private readonly WrapperData DataProvider = new WrapperData();
        private const string litedbloc = "wrapper_test.db";

        [Fact]
        public void Should_Be_Able_To_Initialize()
        {
            ICollectionRef<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "insert_collection");
            Assert.NotNull(reference);
            Assert.NotNull(reference.Config);
        }

        [Fact]
        public void Should_Error_On_Wrong_Config()
        {
            try
            {
                ICollectionRef<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, string.Empty);
                Assert.Null(reference);
            }
            catch (Exception ex)
            { Assert.False(string.IsNullOrWhiteSpace(ex.Message)); }
        }

        private void VerifyAssertModels(WrapperModel basis, WrapperModel target)
        {
            Assert.NotNull(target);
            Assert.Equal(basis._ID, target._ID);
            Assert.Equal(basis.Word, target.Word);
            Assert.Equal(basis.Number, target.Number);
            Assert.Equal(basis.Added.Date, target.Added.Date);
        }

        [Fact]
        public async Task Can_Insert_Item()
        {
            try
            {
                ICollectionRef<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "insert_collection");
                reference.Insert(DataProvider.GetModel());
                await reference.Commit();
                reference.Drop();
            }
            catch (Exception ex)
            { Assert.Null(ex); }
        }

        [Fact]
        public async Task Can_Insert_Multiple_Items()
        {
            try
            {
                ICollectionRef<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "insert_collection");
                reference.Insert(DataProvider.GetModel(20));
                await reference.Commit();
                reference.Drop();
            }
            catch (Exception ex)
            { Assert.Null(ex); }
        }

        [Fact]
        public async Task Can_Update_Item()
        {
            try
            {
                ICollectionRef<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "update_collection");
                using (WrapperModel _model = DataProvider.GetModel())
                {
                    reference.Insert(_model);
                    await reference.Commit();
                    using WrapperModel _inserted = reference.Get(_model._ID);
                    _inserted.Word = DataProvider.Word();
                    _inserted.Number = DataProvider.Number();
                    reference.Update(_inserted);
                    await reference.Commit();
                    using WrapperModel _updated = reference.Get(_inserted._ID);
                    VerifyAssertModels(_inserted, _updated);
                }
                reference.Drop();
            }
            catch (Exception ex)
            { Assert.Null(ex); }
        }

        [Fact]
        public async Task Can_Update_Multiple_Items()
        {
            try
            {
                ICollectionRef<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "update_collection");
                reference.Insert(DataProvider.GetModel(10));
                await reference.Commit();

                PagedResult<WrapperModel> _pagedForUpdate = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));
                foreach (WrapperModel _model in _pagedForUpdate.Result)
                {
                    _model.Word = DataProvider.Word();
                    _model.Number = DataProvider.Number();
                }
                reference.Update(_pagedForUpdate.Result);
                await reference.Commit();

                PagedResult<WrapperModel> _pagedForChecking = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));
                foreach (WrapperModel _model in _pagedForChecking.Result)
                    VerifyAssertModels(_pagedForUpdate.Result[_pagedForChecking.Result.IndexOf(_model)], _model);

                reference.Drop();
            }
            catch (Exception ex)
            { Assert.Null(ex); }
        }

        [Fact]
        public async Task Can_Remove_Item()
        {
            try
            {
                ICollectionRef<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "delete_collection");
                using WrapperModel _model = DataProvider.GetModel();
                reference.Insert(_model);
                await reference.Commit();
                reference.Remove(_model._ID);
                await reference.Commit();
                using WrapperModel _deletedModel = reference.Get(_model._ID);
                Assert.Null(_deletedModel);
                reference.Drop();
            }
            catch (Exception ex)
            { Assert.Null(ex); }
        }

        [Fact]
        public async Task Can_Remove_Multiple_Items()
        {
            try
            {
                ICollectionRef<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "delete_collection");
                reference.Insert(DataProvider.GetModel(10));
                await reference.Commit();

                PagedResult<WrapperModel> _pagedForDelete = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));
                reference.Remove(_pagedForDelete.Result.Select(_d => _d._ID).ToList());
                await reference.Commit();

                PagedResult<WrapperModel> _pagedForChecking = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));
                Assert.Equal(0, _pagedForChecking.TotalRows);
                Assert.False(_pagedForChecking.Result.Any());

                reference.Drop();
            }
            catch (Exception ex)
            { Assert.Null(ex); }
        }
    }
}
