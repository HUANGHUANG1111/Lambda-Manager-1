using System.ComponentModel;

namespace Lambda
{
    public enum Severity
    {
        [Description("���ԣ�")]
        DEBUG,
        [Description("��Ϣ��")]
        INFO,
        [Description("�澯��")]
        WARNING,
        [Description("����")]
        ERROR,
        [Description("���ش���")]
        FATAL_ERROR
    }

    public class Message
    {
        public Severity Severity { get; set; }

        public string? Text { get; set; }
    }
}



