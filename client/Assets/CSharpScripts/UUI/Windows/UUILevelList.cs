using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UGameTools;
using Proto;

namespace Windows
{
    partial class UUILevelList
    {
        public class ContentTableModel : TableItemModel<ContentTableTemplate>
        {
            public ContentTableModel(){}
            public override void InitModel()
            {
                this.Template.Button =this.Item.Root.GetComponent<Button>();
                //todo
                this.Template.Button.onClick.AddListener(()=>{
                    if(Onclick ==null) return;
                    Onclick(this);
                });
            }

            public Action<ContentTableModel> Onclick;
            public ExcelConfig.BattleLevelData Data{ set; get; }

            public void SetLevel(ExcelConfig.BattleLevelData level)
            {
                Data = level;
                this.Template.Button.SetText(level.Name);
            }
        }

        protected override void InitModel()
        {
            base.InitModel();
            Bt_Return.onClick.AddListener(() =>
                {
                    this.HideWindow();
                });
            //Write Code here
        }
        protected override void OnShow()
        {
            base.OnShow();
            var levels = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.BattleLevelData>();
            ContentTableManager.Count = levels.Length;
            int index = 0;
            foreach (var i in ContentTableManager)
            {
                i.Model.SetLevel(levels[index]);
                i.Model.Onclick = OnItemClick;
                index++;
            }



            ScrollView.SetLayoutVertical();
        }

        private void OnItemClick(ContentTableModel item)
        {
            var gate = UAppliaction.Singleton.GetGate() as GMainGate;
            if (gate == null)
                return;
            var request = gate.Client.CreateRequest<C2G_BeginGame,G2C_BeginGame>();
            request.RequestMessage.MapID = 1;

            request.OnCompleted = (s, r) =>
            {   
                if (r.Code == ErrorCode.OK)
                {
                   UAppliaction.Singleton.GotoBattleGate(r.ServerInfo, item.Data.ID);
                }
                else
                {
                    UAppliaction.Singleton.ShowError(r.Code);   
                }
            };
            request.SendRequest();
        }
           

        protected override void OnHide()
        {
            base.OnHide();
        }
    }
}