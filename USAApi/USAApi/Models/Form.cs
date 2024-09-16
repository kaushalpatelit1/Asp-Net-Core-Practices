namespace USAApi.Models
{
    public class Form : Collection<FormField>
    {
        public const string Relation = "form";
        public const string EditRelation = "edit-form";
        public const string CreateRalation = "create-form";
        public const string QueryRelation = "query-form";
    }
}
