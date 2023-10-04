using UnityEngine;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;

public class ExcelWriter : MonoBehaviour
{
    public string filePath; // Excel�t�@�C���̖��O�ƃp�X
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        // �f�[�^��Excel�t�@�C���ɏ�������
        WriteDataToExcel();
    }

    // �f�[�^��Excel�t�@�C���ɏ������ފ֐�
    void WriteDataToExcel()
    {
        IWorkbook workbook;
        // �V�������[�N�u�b�N���쐬
        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            workbook = new XSSFWorkbook(stream);
            stream.Close();
        }


        //IWorkbook workbook = new XSSFWorkbook();
        int i = 4;

        // ���[�N�V�[�g���쐬
        ISheet sheet = workbook.CreateSheet(name);

        // �f�[�^�����̃Z���ɏ�������
        sheet.CreateRow(i).CreateCell(0).SetCellValue("Name");
        sheet.GetRow(i).CreateCell(1).SetCellValue("Score");

        sheet.CreateRow(1).CreateCell(0).SetCellValue("Player1");
        sheet.GetRow(1).CreateCell(1).SetCellValue(100);

        sheet.CreateRow(2).CreateCell(0).SetCellValue("Player2");
        sheet.GetRow(2).CreateCell(1).SetCellValue(150);

        sheet.CreateRow(3).CreateCell(0).SetCellValue("Player3");
        sheet.GetRow(3).CreateCell(1).SetCellValue(75);

        // Excel�t�@�C����ۑ�
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            workbook.Write(fs);
            fs.Close();
        }

        Debug.Log("Excel�t�@�C�����쐬����܂���: " + filePath);
    }
}

