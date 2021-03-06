﻿using PipedriveSqlSync.Users;
using PipeDriveApi;

namespace PipedriveSqlSync.Persons
{
    public class DbPerson : Person
    {
        public int DbOwnerId
        {
            get { return OwnerId?.Id ?? -1; }
            set { OwnerId = new Owner { Id = value }; }
        }

        public DbUser Owner { get; set; }

        public int DbOrgId
        {
            get { return OrgId?.Id ?? -1; }
            set { OrgId = new OrganizationId { Id = value }; }
        }

        public int? DbPictureId
        {
            get { return PictureId?.Id; }
            set { PictureId = value.HasValue ? new PictureId { Id = value.Value } : null; }
        }
    }
}