namespace PipedriveSqlSync.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CompanyId = c.Int(),
                        UserId = c.Int(),
                        Done = c.Boolean(nullable: false),
                        Type = c.String(),
                        ReferenceType = c.String(),
                        ReferenceId = c.Int(),
                        DueDate = c.DateTime(),
                        DueTime = c.DateTime(),
                        Duration = c.String(),
                        AddTime = c.DateTime(),
                        MarkedAsDoneTime = c.DateTime(),
                        Subject = c.String(),
                        DealId = c.Int(),
                        OrgId = c.Int(),
                        PersonId = c.Int(),
                        ActiveFlag = c.Boolean(nullable: false),
                        UpdateTime = c.DateTime(),
                        PersonName = c.String(),
                        OrgName = c.String(),
                        Note = c.String(),
                        DealTitle = c.String(),
                        AssignedToUserId = c.Int(),
                        CreatedByUserId = c.Int(),
                        OwnerName = c.String(),
                        PersonDropboxBcc = c.String(),
                        DealDropboxBcc = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deals", t => t.DealId)
                .ForeignKey("dbo.Organizations", t => t.OrgId)
                .ForeignKey("dbo.Persons", t => t.PersonId)
                .Index(t => t.DealId)
                .Index(t => t.OrgId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        OrgId = c.Int(nullable: false),
                        StageId = c.Int(nullable: false),
                        Title = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.String(),
                        AddTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        StageChangeTime = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Status = c.String(),
                        NextActivityDate = c.DateTime(),
                        NextActivityTime = c.DateTime(),
                        NextActivityId = c.Int(),
                        LastActivityId = c.Int(),
                        LastActivityDate = c.DateTime(),
                        LostReason = c.String(),
                        VisibleTo = c.String(),
                        CloseTime = c.DateTime(),
                        PipelineId = c.Int(),
                        WonTime = c.DateTime(),
                        FirstWonTime = c.DateTime(),
                        LostTime = c.DateTime(),
                        ProductsCount = c.Int(nullable: false),
                        FilesCount = c.Int(nullable: false),
                        NotesCount = c.Int(nullable: false),
                        FollowersCount = c.Int(nullable: false),
                        EmailMessagesCount = c.Int(nullable: false),
                        DoneActivitiesCount = c.Int(nullable: false),
                        UndoneActivitiesCount = c.Int(nullable: false),
                        ReferenceActivitiesCount = c.Int(nullable: false),
                        ParticipantsCount = c.Int(nullable: false),
                        ExpectedCloseDate = c.DateTime(),
                        LastIncomingMailTime = c.DateTime(),
                        LastOutgoingMailTime = c.DateTime(),
                        StageOrderNr = c.Int(),
                        PersonName = c.String(),
                        OrgName = c.String(),
                        NextActivitySubject = c.String(),
                        NextActivityType = c.String(),
                        NextActivityDuration = c.String(),
                        NextActivityNote = c.String(),
                        FormattedValue = c.String(),
                        RottenTime = c.DateTime(),
                        WeightedValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FormattedWeightedValue = c.String(),
                        OwnerName = c.String(),
                        CcEmail = c.String(),
                        OrgHidden = c.Boolean(nullable: false),
                        PersonHidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrgId)
                .ForeignKey("dbo.Persons", t => t.PersonId)
                .Index(t => t.PersonId)
                .Index(t => t.OrgId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Name = c.String(),
                        OpenDealsCount = c.Int(nullable: false),
                        RelatedOpenDealsCount = c.Int(nullable: false),
                        ClosedDealsCount = c.Int(nullable: false),
                        RelatedClosedDealsCount = c.Int(nullable: false),
                        EmailMessagesCount = c.Int(nullable: false),
                        PeopleCount = c.Int(nullable: false),
                        ActivitiesCount = c.Int(nullable: false),
                        DoneActivitiesCount = c.Int(nullable: false),
                        UndoneActivitiesCount = c.Int(nullable: false),
                        ReferenceActivitiesCount = c.Int(nullable: false),
                        FilesCount = c.Int(nullable: false),
                        NotesCount = c.Int(nullable: false),
                        FollowersCount = c.Int(nullable: false),
                        WonDealsCount = c.Int(nullable: false),
                        ReltatedWonDealsCount = c.Int(nullable: false),
                        LostDealsCount = c.Int(nullable: false),
                        RelatedLostDealsCount = c.Int(nullable: false),
                        ActiveFlag = c.Boolean(nullable: false),
                        CategoryId = c.Int(),
                        PictureId = c.String(),
                        CountryCode = c.String(),
                        FirstChar = c.String(),
                        UpdateTime = c.DateTime(),
                        AddTime = c.DateTime(),
                        VisibleTo = c.String(),
                        NextActivityDate = c.DateTime(),
                        NextActivityTime = c.DateTime(),
                        NextActivityId = c.Int(),
                        LastActivityId = c.Int(),
                        LastActivityDate = c.DateTime(),
                        Address = c.String(),
                        AddressSubpremise = c.String(),
                        AddressStreetNumber = c.String(),
                        AddressRoute = c.String(),
                        AddressSublocality = c.String(),
                        AddressLocality = c.String(),
                        AddressAdminAreaLevel1 = c.String(),
                        AddressAdminAreaLevel2 = c.String(),
                        AddressCountry = c.String(),
                        AddressPostalCode = c.String(),
                        AddressFormattedAddress = c.String(),
                        OwnerName = c.String(),
                        CcEmail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        OrgId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Name = c.String(),
                        AddTime = c.DateTime(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        OpenDealsCount = c.Int(nullable: false),
                        RelatedOpenDealsCount = c.Int(nullable: false),
                        ClosedDealsCount = c.Int(nullable: false),
                        RelatedClosedDealsCount = c.Int(nullable: false),
                        ParticipantOpenDealsCount = c.Int(nullable: false),
                        ParticipantClosedDealsCount = c.Int(nullable: false),
                        EmailMessagesCount = c.Int(nullable: false),
                        ActivitiesCount = c.Int(nullable: false),
                        DoneActivitiesCount = c.Int(nullable: false),
                        UndoneActivitiesCount = c.Int(nullable: false),
                        ReferenceActivitiesCount = c.Int(nullable: false),
                        FilesCount = c.Int(nullable: false),
                        NotesCount = c.Int(nullable: false),
                        FollowersCount = c.Int(nullable: false),
                        WonDealsCount = c.Int(nullable: false),
                        RelatedWonDealsCount = c.Int(nullable: false),
                        LostDealsCount = c.Int(nullable: false),
                        RelatedLostDealsCount = c.Int(nullable: false),
                        ActiveFlag = c.Boolean(nullable: false),
                        UpdateTime = c.DateTime(),
                        VisibleTo = c.String(),
                        PictureId = c.String(),
                        NextActivityDate = c.DateTime(),
                        NextActivityTime = c.DateTime(),
                        NextActivityId = c.Int(),
                        LastActivityId = c.Int(),
                        LastActivityDate = c.DateTime(),
                        LastIncomingMailTime = c.DateTime(),
                        LastOutgoingMailTime = c.DateTime(),
                        OrgName = c.String(),
                        OwnerName = c.String(),
                        CcEmail = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrgId)
                .Index(t => t.OrgId);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DealId = c.Int(),
                        PersonId = c.Int(),
                        OrgId = c.Int(),
                        Content = c.String(),
                        AddTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        ActiveFlag = c.Boolean(nullable: false),
                        PinnedToDeal = c.Boolean(nullable: false),
                        PinnedToPerson = c.Boolean(nullable: false),
                        PinnedToOrganization = c.Boolean(nullable: false),
                        LastUpdateUserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deals", t => t.DealId)
                .ForeignKey("dbo.Organizations", t => t.OrgId)
                .ForeignKey("dbo.Persons", t => t.PersonId)
                .Index(t => t.DealId)
                .Index(t => t.PersonId)
                .Index(t => t.OrgId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Notes", "OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Notes", "DealId", "dbo.Deals");
            DropForeignKey("dbo.Activities", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Activities", "OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Activities", "DealId", "dbo.Deals");
            DropForeignKey("dbo.Deals", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Persons", "OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Deals", "OrgId", "dbo.Organizations");
            DropIndex("dbo.Notes", new[] { "OrgId" });
            DropIndex("dbo.Notes", new[] { "PersonId" });
            DropIndex("dbo.Notes", new[] { "DealId" });
            DropIndex("dbo.Persons", new[] { "OrgId" });
            DropIndex("dbo.Deals", new[] { "OrgId" });
            DropIndex("dbo.Deals", new[] { "PersonId" });
            DropIndex("dbo.Activities", new[] { "PersonId" });
            DropIndex("dbo.Activities", new[] { "OrgId" });
            DropIndex("dbo.Activities", new[] { "DealId" });
            DropTable("dbo.Notes");
            DropTable("dbo.Persons");
            DropTable("dbo.Organizations");
            DropTable("dbo.Deals");
            DropTable("dbo.Activities");
        }
    }
}
