namespace UserActivity.CL.WPF.Entities.RDF
{
    public interface IRDFMapper
    {
        RDFSession MapToRDF(Session session);

        Session MapFromRDF(RDFSession session);
    }
}