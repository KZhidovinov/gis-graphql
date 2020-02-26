using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class AddWayShapesView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE VIEW [dbo].[WayShapes]
AS
WITH data AS (
    SELECT
        d.Id,
        string_agg(
            CAST(d.point.STX AS nvarchar(50)) + ' ' + CAST(d.point.STY AS nvarchar(50)),
            ','
        ) WITHIN GROUP (
            ORDER BY
                d.WayIdx
        ) AS wkt
    FROM
        (
            SELECT
                w.Id,
                wn.WayIdx,
                n.Location AS point
            FROM
                Ways w
                JOIN WayNodes wn ON wn.WayId = w.Id
                JOIN Nodes n ON wn.NodeId = n.Id
        ) AS d
    GROUP BY
        d.Id
    HAVING
        COUNT(d.point) > 1
)
SELECT
    g.Id,
    geometry :: STGeomFromText('LINESTRING(' + g.wkt + ')', 4326) AS shape
FROM
    data g;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [dbo].[WayShapes]");
        }
    }
}
