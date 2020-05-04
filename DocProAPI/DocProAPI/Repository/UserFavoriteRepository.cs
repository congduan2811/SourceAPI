using DocProModel.Customs.Params;
using DocProModel.Models;
using DocProUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocProAPI.Repository
{
    public class UserFavoriteRepository : DocProModel.Repository.UserFavoriteRepository
    {
        public static List<UserFavorite> GetList(int idChannel, int idUser, FavoriteType favoriteType, long[] idDocs = null)
        {
            return Instance.Search(idChannel, new List<CondParam>
            {
                new CondParam
                {
                    FieldName = "CreatedBy",
                    Value = idUser
                },
                new CondParam
                {
                    FieldName = "Type",
                    Value = (int)favoriteType
                },
                new CondParam
                {
                    FieldName = "IDDoc",
                    Operator = CondOperator.In,
                    Value = idDocs
                }
            });
        }
        public static List<UserFavorite> GetList(int idChannel, int idUser, FavoriteType favoriteType)
        {
            return Instance.Search(idChannel, new List<CondParam>
            {
                new CondParam
                {
                    FieldName = "CreatedBy",
                    Value = idUser
                },
                new CondParam
                {
                    FieldName = "Type",
                    Value = (int)favoriteType //FavoriteType.Stgfile
                }
            });
        }
    }
}