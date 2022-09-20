using Microsoft.EntityFrameworkCore.Migrations;

namespace INSEE.KIOSK.API.Migrations
{
    public partial class VW_DailyGuestSummaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW VW_GuestSummary 
AS 
SELECT header.Name AS PersonName, header.NIC AS PersonNIC, detail.ProfileImg AS PersonProfileImage, detail.Type AS Reason, sites.Code AS SiteCode, sites.Name AS SiteName, sites.Location AS SiteLocation, detail.InsertedDateTime 
                  AS inserted, company.Name AS Company, (CASE WHEN attempt.status IS NULL 
                  THEN 'FAILED' ELSE attempt.Status END) AS ExamStatus, (CASE WHEN attempt.TotalMarks IS NULL THEN 0 ELSE attempt.TotalMarks END) AS ExamTotalMarks, attempt.TestCompletedDateTime AS ExamCompleted
from T_Guest_Detail as detail
inner join T_Guest_Master as header on detail.FK_GuestMasterCode = header.code
inner join M_Site as sites on detail.FK_SiteCode = sites.Code
inner join M_Company as company on sites.FK_CompanyCode = company.Code
LEFT OUTER join  (SELECT a.FK_GuestDetailCode, a.TestCompletedDateTime, a.TotalMarks, a.Status
FROM T_Guest_Detail_Attempt a
 inner join(
    SELECT FK_GuestDetailCode, MAX(TestCompletedDateTime) inserted
    FROM T_Guest_Detail_Attempt 
    GROUP BY FK_GuestDetailCode
) b ON a.FK_GuestDetailCode = b.FK_GuestDetailCode AND a.TestCompletedDateTime = b.inserted) as attempt on detail.Code = attempt.FK_GuestDetailCode 

");
            migrationBuilder.Sql(@"Create View VW_DailyGuestSummary
as
SELECT *
FROM VW_GuestSummary a
INNER JOIN (
    SELECT PersonNIC as x1, MAX(inserted)  as x
    FROM VW_GuestSummary 
    GROUP BY PersonNIC,CONVERT(date,  inserted)
) b ON a.PersonNIC = b.x1 AND a.inserted = b.x");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop view VW_DailyGuestSummary");
            migrationBuilder.Sql(@"drop view    VW_GuestSummary");
        }

    }
}
