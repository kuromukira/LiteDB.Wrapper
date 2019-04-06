using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            CollectionReference<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "insert_collection");
            Assert.NotNull(reference);
            Assert.NotNull(reference.Config);
        }

        [Fact]
        public void Should_Error_On_Wrong_Config()
        {
            try
            {
                CollectionReference<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, string.Empty);
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
                CollectionReference<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "insert_collection");
                reference.Add(DataProvider.GetModel());
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
                CollectionReference<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "insert_collection");
                reference.Add(DataProvider.GetModel(20));
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
                CollectionReference<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "update_collection");
                using (WrapperModel _model = DataProvider.GetModel())
                {
                    reference.Add(_model);
                    await reference.Commit();
                    using (WrapperModel _inserted = reference.Get(_model._ID))
                    {
                        _inserted.Word = DataProvider.Word();
                        _inserted.Number = DataProvider.Number();
                        reference.Modify(_inserted);
                        await reference.Commit();
                        using (WrapperModel _updated = reference.Get(_inserted._ID))
                        {
                            VerifyAssertModels(_inserted, _updated);
                        }
                    }
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
                CollectionReference<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "update_collection");
                reference.Add(DataProvider.GetModel(10));
                await reference.Commit();

                (IList<WrapperModel> _forUpdate, long r1) = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));
                foreach (WrapperModel _model in _forUpdate)
                {
                    _model.Word = DataProvider.Word();
                    _model.Number = DataProvider.Number();
                }
                reference.Modify(_forUpdate);
                await reference.Commit();

                (IList<WrapperModel> _forChecking, long r2) = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));

                foreach (WrapperModel _model in _forChecking)
                    VerifyAssertModels(_forUpdate[_forChecking.IndexOf(_model)], _model);

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
                CollectionReference<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "delete_collection");
                using (WrapperModel _model = DataProvider.GetModel())
                {
                    reference.Add(_model);
                    await reference.Commit();
                    reference.Remove(_model._ID);
                    await reference.Commit();
                    using (WrapperModel _deletedModel = reference.Get(_model._ID))
                    {
                        Assert.Null(_deletedModel);
                    }
                }
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
                CollectionReference<WrapperModel> reference = new CollectionReference<WrapperModel>(litedbloc, "delete_collection");
                reference.Add(DataProvider.GetModel(10));
                await reference.Commit();

                (IList<WrapperModel> _forDelete, long rows) = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));
                reference.Remove(_forDelete.Select(_d => _d._ID).ToList());
                await reference.Commit();

                (IList<WrapperModel> _forChecking, long zeroRows) = reference.GetPaged(new PageOptions(0, 10), new SortOptions(SortOptions.Order.DSC, "_id"));
                Assert.Equal(0, zeroRows);
                Assert.False(_forChecking.Any());

                reference.Drop();
            }
            catch (Exception ex)
            { Assert.Null(ex); }
        }
    }
}
