namespace CurrencyConverterTests.TestFiles
{
    public class TestDailySource
    {
        public string CorrectSource = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<gesmes:Envelope xmlns:gesmes=\"http://www.gesmes.org/xml/2002-08-01\" xmlns=\"http://www.ecb.int/vocabulary/2002-08-01/eurofxref\">" +
            "	<gesmes:subject>Reference rates</gesmes:subject>" +
            "	<gesmes:Sender>" +
            "		<gesmes:name>European Central Bank</gesmes:name>" +
            "	</gesmes:Sender>" +
            "	<Cube>" +
            "		<Cube time='2020-07-16'>" +
            "			<Cube currency='USD' rate='1.1414'/>" +
            "		</Cube>" +
            "	</Cube>" +
            "</gesmes:Envelope>";

        public string NoCurrenciesSource = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<gesmes:Envelope xmlns:gesmes=\"http://www.gesmes.org/xml/2002-08-01\" xmlns=\"http://www.ecb.int/vocabulary/2002-08-01/eurofxref\">" +
            "	<gesmes:subject>Reference rates</gesmes:subject>" +
            "	<gesmes:Sender>" +
            "		<gesmes:name>European Central Bank</gesmes:name>" +
            "	</gesmes:Sender>" +
            "	<Cube>" +
            "	</Cube>" +
            "</gesmes:Envelope>";

        public string IncorectDailySource = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<gesmes:Envelope xmlns:gesmes=\"http://www.gesmes.org/xml/2002-08-01\" xmlns=\"http://www.ecb.int/vocabulary/2002-08-01/eurofxref\">" +
            "	<gesmes:subject>Reference rates</gesmes:subject>" +
            "	<gesmes:Sender>" +
            "		<gesmes:name>European Central Bank</gesmes:name>" +
            "	</gesmes:Sender>" +
            "	>Cube>" +
            "		<Cube time='2020-07-16'>" +
            "			<Cube currency='USD' rate='1.1414'/>" +
            "		</Cube>" +
            "	</Cube>" +
            "</gesmes:Envelope<";
    }
}
