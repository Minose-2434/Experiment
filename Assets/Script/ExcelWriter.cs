using UnityEngine;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;

public class ExcelWriter : MonoBehaviour
{
    public string filePath; // Excelファイルの名前とパス

    public GameObject Sphere;
    Rotate RotateMethod;
    public bool set = true;
    int i = 1;
    IWorkbook workbook;
    ISheet sheet;

    // Start is called before the first frame update
    void Start()
    {
        RotateMethod = Sphere.gameObject.GetComponent<Rotate>();

        // ワークブックを作成
        using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
        {
            workbook = new XSSFWorkbook(stream);
            stream.Close();
        }
        // ワークシートを作成
        sheet = workbook.CreateSheet(RotateMethod.name);

        //ヘッダーの作成
        sheet.CreateRow(0).CreateCell(0).SetCellValue("FlameNum");
        sheet.GetRow(0).CreateCell(1).SetCellValue("x");
        sheet.GetRow(0).CreateCell(2).SetCellValue("y");
        sheet.GetRow(0).CreateCell(3).SetCellValue("z");
        sheet.GetRow(0).CreateCell(4).SetCellValue("pitch");
        sheet.GetRow(0).CreateCell(5).SetCellValue("yaw");
        sheet.GetRow(0).CreateCell(6).SetCellValue("roll");
        sheet.GetRow(0).CreateCell(7).SetCellValue("deltaTime");
    }

    void Update()
    {
        if (RotateMethod.start)
        {
            WriteDataToExcel();
        }

        if (RotateMethod.finish && set)
        {
            // Excelファイルを保存
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                workbook.Write(fs);
                fs.Close();
            }

            Debug.Log("Excelファイルが作成されました: " + filePath);
            set = false;
        }
    }

    // データをExcelファイルに書き込む関数
    void WriteDataToExcel()
    {
        //フレームナンバーを記入
        sheet.CreateRow(i).CreateCell(0).SetCellValue(i);
        sheet.GetRow(i).CreateCell(1).SetCellValue(this.transform.position.x);
        sheet.GetRow(i).CreateCell(2).SetCellValue(this.transform.position.y);
        sheet.GetRow(i).CreateCell(3).SetCellValue(this.transform.position.z);
        sheet.GetRow(i).CreateCell(4).SetCellValue(this.transform.rotation.x);
        sheet.GetRow(i).CreateCell(5).SetCellValue(this.transform.rotation.y);
        sheet.GetRow(i).CreateCell(6).SetCellValue(this.transform.rotation.z);
        sheet.GetRow(i).CreateCell(7).SetCellValue(Time.deltaTime);
        i += 1;
    }
}

