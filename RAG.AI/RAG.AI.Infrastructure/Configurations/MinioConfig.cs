using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG.AI.Infrastructure.Configurations;

public class MinioConfig
{
    public const string Key = "Minio";
    public string Connection { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string RootBucketName { get; set; }
    public bool WithSSl { get; set;} = false;
    public string Prefix { get; set; }
    public string BankFolder { get; set; }
    public string SliderFolder { get; set; }
    public string ProductFolder { get; set; }
    public string FullBankAddress => $"https://{RootBucketName}.storage.c2.liara.space/{BankFolder}/";
    public string FullProductAddress => $"https://{RootBucketName}.storage.c2.liara.space/{ProductFolder}/";
    public string FullSliderAddress => $"https://{RootBucketName}.storage.c2.liara.space/{SliderFolder}/";
}



