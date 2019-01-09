namespace Visualization
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PlayerCardOne = new System.Windows.Forms.PictureBox();
            this.PlayerCardTwo = new System.Windows.Forms.PictureBox();
            this.Testbtn = new System.Windows.Forms.Button();
            this.CommunityOne = new System.Windows.Forms.PictureBox();
            this.CommunityTwo = new System.Windows.Forms.PictureBox();
            this.CommunityThree = new System.Windows.Forms.PictureBox();
            this.CommunityFour = new System.Windows.Forms.PictureBox();
            this.CommunityFive = new System.Windows.Forms.PictureBox();
            this.LogWindow = new System.Windows.Forms.RichTextBox();
            this.PlayerCardsLbl = new System.Windows.Forms.Label();
            this.ComCardsLbl = new System.Windows.Forms.Label();
            this.Next = new System.Windows.Forms.Button();
            this.YourBetlbl = new System.Windows.Forms.Label();
            this.Balancelbl = new System.Windows.Forms.Label();
            this.CurrentBetlbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCardOne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCardTwo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityOne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityTwo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityThree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityFour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityFive)).BeginInit();
            this.SuspendLayout();
            // 
            // PlayerCardOne
            // 
            this.PlayerCardOne.Location = new System.Drawing.Point(12, 300);
            this.PlayerCardOne.Name = "PlayerCardOne";
            this.PlayerCardOne.Size = new System.Drawing.Size(100, 138);
            this.PlayerCardOne.TabIndex = 0;
            this.PlayerCardOne.TabStop = false;
            // 
            // PlayerCardTwo
            // 
            this.PlayerCardTwo.Location = new System.Drawing.Point(118, 300);
            this.PlayerCardTwo.Name = "PlayerCardTwo";
            this.PlayerCardTwo.Size = new System.Drawing.Size(100, 138);
            this.PlayerCardTwo.TabIndex = 1;
            this.PlayerCardTwo.TabStop = false;
            // 
            // Testbtn
            // 
            this.Testbtn.Location = new System.Drawing.Point(762, 12);
            this.Testbtn.Name = "Testbtn";
            this.Testbtn.Size = new System.Drawing.Size(75, 56);
            this.Testbtn.TabIndex = 2;
            this.Testbtn.Text = "Start Test";
            this.Testbtn.UseVisualStyleBackColor = true;
            this.Testbtn.Click += new System.EventHandler(this.Testbtn_Click);
            // 
            // CommunityOne
            // 
            this.CommunityOne.Location = new System.Drawing.Point(118, 105);
            this.CommunityOne.Name = "CommunityOne";
            this.CommunityOne.Size = new System.Drawing.Size(100, 138);
            this.CommunityOne.TabIndex = 3;
            this.CommunityOne.TabStop = false;
            // 
            // CommunityTwo
            // 
            this.CommunityTwo.Location = new System.Drawing.Point(224, 105);
            this.CommunityTwo.Name = "CommunityTwo";
            this.CommunityTwo.Size = new System.Drawing.Size(100, 138);
            this.CommunityTwo.TabIndex = 4;
            this.CommunityTwo.TabStop = false;
            // 
            // CommunityThree
            // 
            this.CommunityThree.Location = new System.Drawing.Point(330, 105);
            this.CommunityThree.Name = "CommunityThree";
            this.CommunityThree.Size = new System.Drawing.Size(100, 138);
            this.CommunityThree.TabIndex = 5;
            this.CommunityThree.TabStop = false;
            // 
            // CommunityFour
            // 
            this.CommunityFour.Location = new System.Drawing.Point(436, 105);
            this.CommunityFour.Name = "CommunityFour";
            this.CommunityFour.Size = new System.Drawing.Size(100, 138);
            this.CommunityFour.TabIndex = 6;
            this.CommunityFour.TabStop = false;
            // 
            // CommunityFive
            // 
            this.CommunityFive.Location = new System.Drawing.Point(542, 105);
            this.CommunityFive.Name = "CommunityFive";
            this.CommunityFive.Size = new System.Drawing.Size(100, 138);
            this.CommunityFive.TabIndex = 7;
            this.CommunityFive.TabStop = false;
            // 
            // LogWindow
            // 
            this.LogWindow.Location = new System.Drawing.Point(843, 12);
            this.LogWindow.Name = "LogWindow";
            this.LogWindow.Size = new System.Drawing.Size(247, 426);
            this.LogWindow.TabIndex = 8;
            this.LogWindow.Text = "";
            // 
            // PlayerCardsLbl
            // 
            this.PlayerCardsLbl.AutoSize = true;
            this.PlayerCardsLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerCardsLbl.Location = new System.Drawing.Point(12, 277);
            this.PlayerCardsLbl.Name = "PlayerCardsLbl";
            this.PlayerCardsLbl.Size = new System.Drawing.Size(89, 20);
            this.PlayerCardsLbl.TabIndex = 9;
            this.PlayerCardsLbl.Text = "Your Cards";
            this.PlayerCardsLbl.Visible = false;
            // 
            // ComCardsLbl
            // 
            this.ComCardsLbl.AutoSize = true;
            this.ComCardsLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComCardsLbl.Location = new System.Drawing.Point(114, 82);
            this.ComCardsLbl.Name = "ComCardsLbl";
            this.ComCardsLbl.Size = new System.Drawing.Size(134, 20);
            this.ComCardsLbl.TabIndex = 10;
            this.ComCardsLbl.Text = "Community Cards";
            this.ComCardsLbl.Visible = false;
            // 
            // Next
            // 
            this.Next.Location = new System.Drawing.Point(762, 74);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(75, 56);
            this.Next.TabIndex = 12;
            this.Next.Text = "Next";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Visible = false;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // YourBetlbl
            // 
            this.YourBetlbl.AutoSize = true;
            this.YourBetlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YourBetlbl.Location = new System.Drawing.Point(648, 133);
            this.YourBetlbl.Name = "YourBetlbl";
            this.YourBetlbl.Size = new System.Drawing.Size(133, 20);
            this.YourBetlbl.TabIndex = 13;
            this.YourBetlbl.Text = "Your Current Bet:";
            this.YourBetlbl.Visible = false;
            // 
            // Balancelbl
            // 
            this.Balancelbl.AutoSize = true;
            this.Balancelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Balancelbl.Location = new System.Drawing.Point(710, 173);
            this.Balancelbl.Name = "Balancelbl";
            this.Balancelbl.Size = new System.Drawing.Size(71, 20);
            this.Balancelbl.TabIndex = 14;
            this.Balancelbl.Text = "Balance:";
            this.Balancelbl.Visible = false;
            // 
            // CurrentBetlbl
            // 
            this.CurrentBetlbl.AutoSize = true;
            this.CurrentBetlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentBetlbl.Location = new System.Drawing.Point(686, 153);
            this.CurrentBetlbl.Name = "CurrentBetlbl";
            this.CurrentBetlbl.Size = new System.Drawing.Size(95, 20);
            this.CurrentBetlbl.TabIndex = 15;
            this.CurrentBetlbl.Text = "Current Bet:";
            this.CurrentBetlbl.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1102, 450);
            this.Controls.Add(this.CurrentBetlbl);
            this.Controls.Add(this.Balancelbl);
            this.Controls.Add(this.YourBetlbl);
            this.Controls.Add(this.Next);
            this.Controls.Add(this.ComCardsLbl);
            this.Controls.Add(this.PlayerCardsLbl);
            this.Controls.Add(this.LogWindow);
            this.Controls.Add(this.CommunityFive);
            this.Controls.Add(this.CommunityFour);
            this.Controls.Add(this.CommunityThree);
            this.Controls.Add(this.CommunityTwo);
            this.Controls.Add(this.CommunityOne);
            this.Controls.Add(this.Testbtn);
            this.Controls.Add(this.PlayerCardTwo);
            this.Controls.Add(this.PlayerCardOne);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCardOne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCardTwo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityOne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityTwo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityThree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityFour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CommunityFive)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PlayerCardOne;
        private System.Windows.Forms.PictureBox PlayerCardTwo;
        private System.Windows.Forms.Button Testbtn;
        private System.Windows.Forms.PictureBox CommunityOne;
        private System.Windows.Forms.PictureBox CommunityTwo;
        private System.Windows.Forms.PictureBox CommunityThree;
        private System.Windows.Forms.PictureBox CommunityFour;
        private System.Windows.Forms.PictureBox CommunityFive;
        private System.Windows.Forms.RichTextBox LogWindow;
        private System.Windows.Forms.Label PlayerCardsLbl;
        private System.Windows.Forms.Label ComCardsLbl;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Label YourBetlbl;
        private System.Windows.Forms.Label Balancelbl;
        private System.Windows.Forms.Label CurrentBetlbl;
    }
}

