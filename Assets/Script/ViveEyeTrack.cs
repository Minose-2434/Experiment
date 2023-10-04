using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {

            public class ViveEyeTrack : MonoBehaviour
            {
                //⓪取得呼び出し-----------------------------
                //呼び出したデータ格納用の関数
                EyeData eye;
                //-------------------------------------------

                //①瞳孔位置--------------------
                //x,y軸
                //左の瞳孔位置格納用関数
                Vector2 LeftPupil;
                //左の瞳孔位置格納用関数
                Vector2 RightPupil;
                //------------------------------

                //②まぶたの開き具合------------
                //左のまぶたの開き具合格納用関数
                float LeftBlink;
                //右のまぶたの開き具合格納用関数
                float RightBlink;
                //------------------------------

                //③視線情報--------------------
                //origin：起点，direction：レイの方向　x,y,z軸
                //両目の視線格納変数
                Vector3 CombineGazeRayorigin;
                Vector3 CombineGazeRaydirection;
                //左目の視線格納変数
                Vector3 LeftGazeRayorigin;
                Vector3 LeftGazeRaydirection;
                //右目の視線格納変数
                Vector3 RightGazeRayorigin;
                Vector3 RightGazeRaydirection;
                //------------------------------

                //④焦点情報--------------------
                //両目の焦点格納変数
                //レイの始点と方向（多分③の内容と同じ）
                Ray CombineRay;
                /*レイがどこに焦点を合わせたかの情報．Vector3 point : 視線ベクトルと物体の衝突位置，float distance : 見ている物体までの距離，
                   Vector3 normal:見ている物体の面の法線ベクトル，Collider collider : 衝突したオブジェクトのCollider，Rigidbody rigidbody：衝突したオブジェクトのRigidbody，Transform transform：衝突したオブジェクトのTransform*/
                //焦点位置にオブジェクトを出すためにpublicにしています．
                public static FocusInfo CombineFocus;
                //レイの半径
                float CombineFocusradius;
                //レイの最大の長さ
                float CombineFocusmaxDistance;
                //オブジェクトを選択的に無視するために使用されるレイヤー ID
                int CombinefocusableLayer = 0;
                //------------------------------

                public bool set = false;
                public string filePath; // Excelファイルの名前とパス
                public string name;     // シート名
                int i = 1;
                IWorkbook workbook;
                ISheet sheet;

                void Start()
                {
                    // ワークブックを作成
                    using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                    {
                        workbook = new XSSFWorkbook(stream);
                        stream.Close();
                    }
                    // ワークシートを作成
                    sheet = workbook.CreateSheet(name);
                    
                }

                //1フレーム毎に実行
                void Update()
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        //ヘッダーの作成
                        sheet.CreateRow(0).CreateCell(0).SetCellValue("FlameNum");
                        sheet.GetRow(0).CreateCell(1).SetCellValue("LeftPupil(x)");
                        sheet.GetRow(0).CreateCell(2).SetCellValue("LeftPupil(y)");
                        sheet.GetRow(0).CreateCell(3).SetCellValue("RightPupil(x)");
                        sheet.GetRow(0).CreateCell(4).SetCellValue("RightPupil(y)");
                        sheet.GetRow(0).CreateCell(5).SetCellValue("LeftBlink");
                        sheet.GetRow(0).CreateCell(6).SetCellValue("RightBlink");
                        sheet.GetRow(0).CreateCell(7).SetCellValue("CombineGazeRayOrigin(x)");
                        sheet.GetRow(0).CreateCell(8).SetCellValue("CombineGazeRayOrigin(y)");
                        sheet.GetRow(0).CreateCell(9).SetCellValue("CombineGazeRayOrigin(z)");
                        sheet.GetRow(0).CreateCell(10).SetCellValue("CombineGazeRayDirection(x)");
                        sheet.GetRow(0).CreateCell(11).SetCellValue("CombineGazeRayDirection(y)");
                        sheet.GetRow(0).CreateCell(12).SetCellValue("CombineGazeRayDirection(z)");
                        sheet.GetRow(0).CreateCell(13).SetCellValue("LeftGazeRayOrigin(x)");
                        sheet.GetRow(0).CreateCell(14).SetCellValue("LeftGazeRayOrigin(y)");
                        sheet.GetRow(0).CreateCell(15).SetCellValue("LeftGazeRayOrigin(z)");
                        sheet.GetRow(0).CreateCell(16).SetCellValue("LeftGazeRayDirection(x)");
                        sheet.GetRow(0).CreateCell(17).SetCellValue("LeftGazeRayDirection(y)");
                        sheet.GetRow(0).CreateCell(18).SetCellValue("LeftGazeRayDirection(z)");
                        sheet.GetRow(0).CreateCell(19).SetCellValue("RightGazeRayOrigin(x)");
                        sheet.GetRow(0).CreateCell(20).SetCellValue("RightGazeRayOrigin(y)");
                        sheet.GetRow(0).CreateCell(21).SetCellValue("RightGazeRayOrigin(z)");
                        sheet.GetRow(0).CreateCell(22).SetCellValue("RightGazeRayDirection(x)");
                        sheet.GetRow(0).CreateCell(23).SetCellValue("RightGazeRayDirection(y)");
                        sheet.GetRow(0).CreateCell(24).SetCellValue("RightGazeRayDirection(z)");
                        set = true;
                        
                    }
                    if (Input.GetKeyDown(KeyCode.Space))
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

                    if (set)
                    {
                        WriteExcel();
                    }
                }

                void WriteExcel()
                {
                    //フレームナンバーを記入
                    sheet.CreateRow(i).CreateCell(0).SetCellValue(i);

                    //⓪取得呼び出し-----------------------------
                    SRanipal_Eye_API.GetEyeData(ref eye);
                    //-------------------------------------------


                    //①瞳孔位置---------------------（HMDを被ると検知される，目をつぶっても位置は返すが，HMDを外すとと止まる．目をつぶってるときはどこの値返してんのか謎．一応まぶた貫通してるっぽい？？？）
                    //左の瞳孔位置を取得
                    if (SRanipal_Eye.GetPupilPosition(EyeIndex.LEFT, out LeftPupil))
                    {
                        //値が有効なら左の瞳孔位置を表示
                        sheet.GetRow(i).CreateCell(1).SetCellValue(LeftPupil.x);
                        sheet.GetRow(i).CreateCell(2).SetCellValue(LeftPupil.y);
                        //Debug.Log("Left Pupil" + LeftPupil.x + ", " + LeftPupil.y);
                    }
                    //右の瞳孔位置を取得
                    if (SRanipal_Eye.GetPupilPosition(EyeIndex.RIGHT, out RightPupil))
                    {
                        //値が有効なら右の瞳孔位置を表示
                        sheet.GetRow(i).CreateCell(3).SetCellValue(RightPupil.x);
                        sheet.GetRow(i).CreateCell(4).SetCellValue(RightPupil.y);
                        //Debug.Log("Right Pupil" + RightPupil.x + ", " + RightPupil.y);
                    }
                    //------------------------------


                    //②まぶたの開き具合------------（HMDを被ってなくても1が返ってくる？？謎）
                    //左のまぶたの開き具合を取得
                    if (SRanipal_Eye.GetEyeOpenness(EyeIndex.LEFT, out LeftBlink, eye))
                    {
                        //値が有効なら左のまぶたの開き具合を表示
                        sheet.GetRow(i).CreateCell(5).SetCellValue(LeftBlink);
                        //Debug.Log("Left Blink" + LeftBlink);
                    }
                    //右のまぶたの開き具合を取得
                    if (SRanipal_Eye.GetEyeOpenness(EyeIndex.RIGHT, out RightBlink, eye))
                    {
                        //値が有効なら右のまぶたの開き具合を表示
                        sheet.GetRow(i).CreateCell(6).SetCellValue(RightBlink);
                        //Debug.Log("Right Blink" + RightBlink);
                    }
                    //------------------------------


                    //③視線情報--------------------（目をつぶると検知されない）
                    //両目の視線情報が有効なら視線情報を表示origin：起点，direction：レイの方向
                    if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out CombineGazeRayorigin, out CombineGazeRaydirection, eye))
                    {
                        sheet.GetRow(i).CreateCell(7).SetCellValue(CombineGazeRayorigin.x);
                        sheet.GetRow(i).CreateCell(8).SetCellValue(CombineGazeRayorigin.y);
                        sheet.GetRow(i).CreateCell(9).SetCellValue(CombineGazeRayorigin.z);
                        sheet.GetRow(i).CreateCell(10).SetCellValue(CombineGazeRaydirection.x);
                        sheet.GetRow(i).CreateCell(11).SetCellValue(CombineGazeRaydirection.y);
                        sheet.GetRow(i).CreateCell(12).SetCellValue(CombineGazeRaydirection.z);
                        //Debug.Log("COMBINE GazeRayorigin" + CombineGazeRayorigin.x + ", " + CombineGazeRayorigin.y + ", " + CombineGazeRayorigin.z);
                        //Debug.Log("COMBINE GazeRaydirection" + CombineGazeRaydirection.x + ", " + CombineGazeRaydirection.y + ", " + CombineGazeRaydirection.z);
                    }

                    //左目の視線情報が有効なら視線情報を表示origin：起点，direction：レイの方向
                    if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out LeftGazeRayorigin, out LeftGazeRaydirection, eye))
                    {
                        sheet.GetRow(i).CreateCell(13).SetCellValue(LeftGazeRayorigin.x);
                        sheet.GetRow(i).CreateCell(14).SetCellValue(LeftGazeRayorigin.y);
                        sheet.GetRow(i).CreateCell(15).SetCellValue(LeftGazeRayorigin.z);
                        sheet.GetRow(i).CreateCell(16).SetCellValue(LeftGazeRaydirection.x);
                        sheet.GetRow(i).CreateCell(17).SetCellValue(LeftGazeRaydirection.y);
                        sheet.GetRow(i).CreateCell(18).SetCellValue(LeftGazeRaydirection.z);
                        //Debug.Log("Left GazeRayorigin" + LeftGazeRayorigin.x + ", " + LeftGazeRayorigin.y + ", " + LeftGazeRayorigin.z);
                        //Debug.Log("Left GazeRaydirection" + LeftGazeRaydirection.x + ", " + LeftGazeRaydirection.y + ", " + LeftGazeRaydirection.z);
                    }


                    //右目の視線情報が有効なら視線情報を表示origin：起点，direction：レイの方向
                    if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out RightGazeRayorigin, out RightGazeRaydirection, eye))
                    {
                        sheet.GetRow(i).CreateCell(19).SetCellValue(RightGazeRayorigin.x);
                        sheet.GetRow(i).CreateCell(20).SetCellValue(RightGazeRayorigin.y);
                        sheet.GetRow(i).CreateCell(21).SetCellValue(RightGazeRayorigin.z);
                        sheet.GetRow(i).CreateCell(22).SetCellValue(RightGazeRaydirection.x);
                        sheet.GetRow(i).CreateCell(23).SetCellValue(RightGazeRaydirection.y);
                        sheet.GetRow(i).CreateCell(24).SetCellValue(RightGazeRaydirection.z);
                        //Debug.Log("Right GazeRayorigin" + RightGazeRayorigin.x + ", " + RightGazeRayorigin.y + ", " + RightGazeRayorigin.z);
                        //Debug.Log("Right GazeRaydirection" + RightGazeRaydirection.x + ", " + RightGazeRaydirection.y + ", " + RightGazeRaydirection.z);
                    }
                    //------------------------------

                    //④焦点情報--------------------
                    //radius, maxDistance，CombinefocusableLayerは省略可
                    if (SRanipal_Eye.Focus(GazeIndex.COMBINE, out CombineRay, out CombineFocus/*, CombineFocusradius, CombineFocusmaxDistance, CombinefocusableLayer*/))
                    {
                        Debug.Log("Combine Focus Point" + CombineFocus.point.x + ", " + CombineFocus.point.y + ", " + CombineFocus.point.z);
                    }
                    //------------------------------
                    i += 1;
                    Debug.Log(i);
                }
            }
        }
    }
}


