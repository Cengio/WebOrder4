Partial Class Report1

    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim FormattingRule1 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim TableGroup1 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup2 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup3 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup4 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim FormattingRule2 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Report1))
        Dim FormattingRule3 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim FormattingRule4 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim TableGroup5 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup6 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup7 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup8 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup9 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup10 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup11 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup12 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup13 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup14 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup15 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup16 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim QrCodeEncoder1 As Telerik.Reporting.Barcodes.QRCodeEncoder = New Telerik.Reporting.Barcodes.QRCodeEncoder()
        Dim FormattingRule5 As Telerik.Reporting.Drawing.FormattingRule = New Telerik.Reporting.Drawing.FormattingRule()
        Dim Group1 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter2 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter3 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter4 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter5 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter6 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter7 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim ReportParameter8 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.TextBox18 = New Telerik.Reporting.TextBox()
        Me.TextBox1 = New Telerik.Reporting.TextBox()
        Me.TextBox9 = New Telerik.Reporting.TextBox()
        Me.TextBox3 = New Telerik.Reporting.TextBox()
        Me.TextBox5 = New Telerik.Reporting.TextBox()
        Me.TextBox12 = New Telerik.Reporting.TextBox()
        Me.TextBox27 = New Telerik.Reporting.TextBox()
        Me.TextBox30 = New Telerik.Reporting.TextBox()
        Me.TextBox33 = New Telerik.Reporting.TextBox()
        Me.TextBox36 = New Telerik.Reporting.TextBox()
        Me.groupFooterSection = New Telerik.Reporting.GroupFooterSection()
        Me.Table2 = New Telerik.Reporting.Table()
        Me.TextBox51 = New Telerik.Reporting.TextBox()
        Me.TextBox53 = New Telerik.Reporting.TextBox()
        Me.TextBox55 = New Telerik.Reporting.TextBox()
        Me.TextBox50 = New Telerik.Reporting.TextBox()
        Me.groupHeaderSection = New Telerik.Reporting.GroupHeaderSection()
        Me.pageHeaderSection1 = New Telerik.Reporting.PageHeaderSection()
        Me.TextBox7 = New Telerik.Reporting.TextBox()
        Me.PictureBox1 = New Telerik.Reporting.PictureBox()
        Me.TextBox10 = New Telerik.Reporting.TextBox()
        Me.TextBox40 = New Telerik.Reporting.TextBox()
        Me.TextBox41 = New Telerik.Reporting.TextBox()
        Me.TextBox42 = New Telerik.Reporting.TextBox()
        Me.TextBox43 = New Telerik.Reporting.TextBox()
        Me.TextBox44 = New Telerik.Reporting.TextBox()
        Me.TextBox46 = New Telerik.Reporting.TextBox()
        Me.TextBox67 = New Telerik.Reporting.TextBox()
        Me.TextBox45 = New Telerik.Reporting.TextBox()
        Me.TextBox11 = New Telerik.Reporting.TextBox()
        Me.TextBox16 = New Telerik.Reporting.TextBox()
        Me.TextBox17 = New Telerik.Reporting.TextBox()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.Table1 = New Telerik.Reporting.Table()
        Me.TextBox4 = New Telerik.Reporting.TextBox()
        Me.TextBox6 = New Telerik.Reporting.TextBox()
        Me.TextBox8 = New Telerik.Reporting.TextBox()
        Me.TextBox2 = New Telerik.Reporting.TextBox()
        Me.TextBox13 = New Telerik.Reporting.TextBox()
        Me.TextBox28 = New Telerik.Reporting.TextBox()
        Me.TextBox31 = New Telerik.Reporting.TextBox()
        Me.TextBox34 = New Telerik.Reporting.TextBox()
        Me.TextBox37 = New Telerik.Reporting.TextBox()
        Me.CheckBox1 = New Telerik.Reporting.CheckBox()
        Me.pageFooterSection1 = New Telerik.Reporting.PageFooterSection()
        Me.TextBox19 = New Telerik.Reporting.TextBox()
        Me.TextBox61 = New Telerik.Reporting.TextBox()
        Me.TextBox64 = New Telerik.Reporting.TextBox()
        Me.TextBox65 = New Telerik.Reporting.TextBox()
        Me.TextBox66 = New Telerik.Reporting.TextBox()
        Me.Barcode1 = New Telerik.Reporting.Barcode()
        Me.TextBox60 = New Telerik.Reporting.TextBox()
        Me.TextBox63 = New Telerik.Reporting.TextBox()
        Me.Shape4 = New Telerik.Reporting.Shape()
        Me.TextBox14 = New Telerik.Reporting.TextBox()
        Me.TextBox15 = New Telerik.Reporting.TextBox()
        Me.Shape1 = New Telerik.Reporting.Shape()
        Me.ReportFooterSection1 = New Telerik.Reporting.ReportFooterSection()
        Me.TextBox20 = New Telerik.Reporting.TextBox()
        Me.TextBox21 = New Telerik.Reporting.TextBox()
        Me.TextBox22 = New Telerik.Reporting.TextBox()
        Me.TextBox23 = New Telerik.Reporting.TextBox()
        Me.TextBox24 = New Telerik.Reporting.TextBox()
        Me.TextBox25 = New Telerik.Reporting.TextBox()
        Me.TextBox26 = New Telerik.Reporting.TextBox()
        Me.TextBox29 = New Telerik.Reporting.TextBox()
        Me.TextBox32 = New Telerik.Reporting.TextBox()
        Me.TextBox35 = New Telerik.Reporting.TextBox()
        Me.TextBox38 = New Telerik.Reporting.TextBox()
        Me.TextBox47 = New Telerik.Reporting.TextBox()
        Me.TextBox48 = New Telerik.Reporting.TextBox()
        Me.TextBox49 = New Telerik.Reporting.TextBox()
        Me.TextBox52 = New Telerik.Reporting.TextBox()
        Me.TextBox54 = New Telerik.Reporting.TextBox()
        Me.Shape2 = New Telerik.Reporting.Shape()
        Me.TextBox56 = New Telerik.Reporting.TextBox()
        Me.TextBox57 = New Telerik.Reporting.TextBox()
        Me.TextBox58 = New Telerik.Reporting.TextBox()
        Me.Shape3 = New Telerik.Reporting.Shape()
        Me.TextBox59 = New Telerik.Reporting.TextBox()
        Me.TextBox62 = New Telerik.Reporting.TextBox()
        Me.ObjectDataSourceOrdine = New Telerik.Reporting.ObjectDataSource()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        '
        '
        Me.TextBox68 = New Telerik.Reporting.TextBox()
        Me.TextBox68.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(3.501R))
        Me.TextBox68.Name = "TextBox68"
        Me.TextBox68.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.632R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox68.Style.Font.Name = "Tahoma"
        Me.TextBox68.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.TextBox68.Value = "= 'Corriere: ' + Fields.spedizioniere"
        '
        'TextBox18
        '
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.521R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox18.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox18.Style.Font.Bold = True
        Me.TextBox18.StyleName = ""
        '
        'TextBox1
        '
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.081R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox1.Style.Font.Bold = True
        Me.TextBox1.Value = "Codice/Farmadati"
        '
        'TextBox9
        '
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.23R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox9.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox9.Style.Font.Bold = True
        Me.TextBox9.StyleName = ""
        Me.TextBox9.Value = "Descrizione"
        '
        'TextBox3
        '
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.415R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox3.Style.Font.Bold = True
        Me.TextBox3.Value = "Formato"
        '
        'TextBox5
        '
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.587R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox5.Style.Font.Bold = True
        Me.TextBox5.Value = "Lotto"
        '
        'TextBox12
        '
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.155R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox12.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox12.Style.Font.Bold = True
        Me.TextBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.TextBox12.StyleName = ""
        Me.TextBox12.Value = "Q.t�"
        '
        'TextBox27
        '
        Me.TextBox27.Name = "TextBox27"
        Me.TextBox27.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.427R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox27.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox27.Style.Font.Bold = True
        Me.TextBox27.StyleName = ""
        Me.TextBox27.Value = "Prezzo"
        '
        'TextBox30
        '
        Me.TextBox30.Name = "TextBox30"
        Me.TextBox30.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.412R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox30.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox30.Style.Font.Bold = True
        Me.TextBox30.StyleName = ""
        Me.TextBox30.Value = "Sct%"
        '
        'TextBox33
        '
        Me.TextBox33.Name = "TextBox33"
        Me.TextBox33.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.035R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox33.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox33.Style.Font.Bold = True
        Me.TextBox33.StyleName = ""
        Me.TextBox33.Value = "Iva"
        '
        'TextBox36
        '
        Me.TextBox36.Name = "TextBox36"
        Me.TextBox36.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.136R), Telerik.Reporting.Drawing.Unit.Cm(0.639R))
        Me.TextBox36.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox36.Style.Font.Bold = True
        Me.TextBox36.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox36.StyleName = ""
        Me.TextBox36.Value = "Importo"
        '
        'groupFooterSection
        '
        FormattingRule1.Filters.Add(New Telerik.Reporting.Filter("= Fields.quantitaNonDisponibiliCount", Telerik.Reporting.FilterOperator.Equal, "0"))
        FormattingRule1.Style.Visible = False
        Me.groupFooterSection.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule1})
        Me.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.7R)
        Me.groupFooterSection.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.Table2, Me.TextBox50})
        Me.groupFooterSection.Name = "groupFooterSection"
        Me.groupFooterSection.PrintAtBottom = False
        '
        'Table2
        '
        Me.Table2.Bindings.Add(New Telerik.Reporting.Binding("DataSource", "= Fields.quantitaNonDisponibili"))
        Me.Table2.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(4.086R)))
        Me.Table2.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(6.274R)))
        Me.Table2.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(4.033R)))
        Me.Table2.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.609R)))
        Me.Table2.Body.SetCellContent(0, 0, Me.TextBox51)
        Me.Table2.Body.SetCellContent(0, 1, Me.TextBox53)
        Me.Table2.Body.SetCellContent(0, 2, Me.TextBox55)
        TableGroup1.Name = "tableGroup3"
        TableGroup2.Name = "tableGroup4"
        TableGroup3.Name = "tableGroup5"
        Me.Table2.ColumnGroups.Add(TableGroup1)
        Me.Table2.ColumnGroups.Add(TableGroup2)
        Me.Table2.ColumnGroups.Add(TableGroup3)
        Me.Table2.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.TextBox51, Me.TextBox53, Me.TextBox55})
        Me.Table2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.6R), Telerik.Reporting.Drawing.Unit.Cm(0.9R))
        Me.Table2.Name = "Table2"
        TableGroup4.Groupings.Add(New Telerik.Reporting.Grouping(Nothing))
        TableGroup4.Name = "detailTableGroup1"
        Me.Table2.RowGroups.Add(TableGroup4)
        Me.Table2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.393R), Telerik.Reporting.Drawing.Unit.Cm(0.609R))
        Me.Table2.Style.Font.Name = "Tahoma"
        Me.Table2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'TextBox51
        '
        Me.TextBox51.Name = "TextBox51"
        Me.TextBox51.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.086R), Telerik.Reporting.Drawing.Unit.Cm(0.609R))
        Me.TextBox51.Value = "=ItemCode"
        '
        'TextBox53
        '
        Me.TextBox53.Name = "TextBox53"
        Me.TextBox53.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.274R), Telerik.Reporting.Drawing.Unit.Cm(0.609R))
        Me.TextBox53.Value = "=Descrizione"
        '
        'TextBox55
        '
        Me.TextBox55.Name = "TextBox55"
        Me.TextBox55.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.033R), Telerik.Reporting.Drawing.Unit.Cm(0.609R))
        Me.TextBox55.Value = "=Quantity"
        '
        'TextBox50
        '
        FormattingRule2.Filters.Add(New Telerik.Reporting.Filter("= Count(Fields.quantitaNonDisponibili)", Telerik.Reporting.FilterOperator.Equal, "0"))
        FormattingRule2.Style.Visible = False
        Me.TextBox50.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule2})
        Me.TextBox50.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.6R), Telerik.Reporting.Drawing.Unit.Cm(0.3R))
        Me.TextBox50.Name = "TextBox50"
        Me.TextBox50.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.867R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox50.Style.Font.Bold = True
        Me.TextBox50.Style.Font.Name = "Tahoma"
        Me.TextBox50.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.TextBox50.Value = "Articoli non disponibili "
        '
        'groupHeaderSection
        '
        Me.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.4R)
        Me.groupHeaderSection.Name = "groupHeaderSection"
        '
        'pageHeaderSection1
        '
        Me.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(6.3R)
        Me.pageHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.TextBox7, Me.PictureBox1, Me.TextBox10, Me.TextBox40, Me.TextBox41, Me.TextBox42, Me.TextBox43, Me.TextBox44, Me.TextBox46, Me.TextBox67, Me.TextBox45, Me.TextBox11, Me.TextBox16, Me.TextBox17})
        Me.pageHeaderSection1.Name = "pageHeaderSection1"
        Me.pageHeaderSection1.PrintOnFirstPage = True
        Me.pageHeaderSection1.PrintOnLastPage = True
        Me.pageHeaderSection1.Style.Font.Name = "Tahoma"
        Me.pageHeaderSection1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        '
        'TextBox7
        '
        Me.TextBox7.CanGrow = False
        Me.TextBox7.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.6R), Telerik.Reporting.Drawing.Unit.Cm(0.2R))
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.2R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11.0R)
        Me.TextBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox7.Value = "='ORDINE NR. '+Fields.Code+' del '+ Fields.OrderDate"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.2R), Telerik.Reporting.Drawing.Unit.Cm(0.2R))
        Me.PictureBox1.MimeType = "image/png"
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.7R), Telerik.Reporting.Drawing.Unit.Cm(1.8R))
        Me.PictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional
        Me.PictureBox1.Value = CType(resources.GetObject("PictureBox1.Value"), Object)
        '
        'TextBox10
        '
        Me.TextBox10.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5R), Telerik.Reporting.Drawing.Unit.Cm(4.2R))
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.3R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox10.Value = "= Fields.indirizzospedizione"
        '
        'TextBox40
        '
        Me.TextBox40.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5R), Telerik.Reporting.Drawing.Unit.Cm(4.8R))
        Me.TextBox40.Name = "TextBox40"
        Me.TextBox40.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.3R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox40.Value = "= Fields.capspedizione+' '+Fields.cittaspedizione"
        '
        'TextBox41
        '
        Me.TextBox41.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5R), Telerik.Reporting.Drawing.Unit.Cm(3.6R))
        Me.TextBox41.Name = "TextBox41"
        Me.TextBox41.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.3R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox41.Value = "= Fields.nomespedizione"
        '
        'TextBox42
        '
        Me.TextBox42.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.2R), Telerik.Reporting.Drawing.Unit.Cm(3.6R))
        Me.TextBox42.Name = "TextBox42"
        Me.TextBox42.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox42.Value = "= Fields.nomecliente"
        '
        'TextBox43
        '
        Me.TextBox43.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.2R), Telerik.Reporting.Drawing.Unit.Cm(4.2R))
        Me.TextBox43.Name = "TextBox43"
        Me.TextBox43.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox43.Value = "= Fields.indirizzocliente"
        '
        'TextBox44
        '
        Me.TextBox44.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.2R), Telerik.Reporting.Drawing.Unit.Cm(4.8R))
        Me.TextBox44.Name = "TextBox44"
        Me.TextBox44.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox44.Value = "= Fields.capcliente + ' ' +Fields.cittacliente"
        '
        'TextBox46
        '
        Me.TextBox46.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.2R), Telerik.Reporting.Drawing.Unit.Cm(5.401R))
        Me.TextBox46.Name = "TextBox46"
        Me.TextBox46.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.632R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox46.Value = "='Tel.' + Fields.telefono+' Fax '+Fields.fax+' Cel. '+Fields.cellulare"
        '
        'TextBox67
        '
        Me.TextBox67.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.202R), Telerik.Reporting.Drawing.Unit.Cm(3.0R))
        Me.TextBox67.Name = "TextBox67"
        Me.TextBox67.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.998R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox67.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.TextBox67.Value = "Indirizzo di fatturazione"
        '
        'TextBox45
        '
        Me.TextBox45.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5R), Telerik.Reporting.Drawing.Unit.Cm(3.0R))
        Me.TextBox45.Name = "TextBox45"
        Me.TextBox45.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.998R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox45.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        Me.TextBox45.Value = "Indirizzo di spedizione"
        '
        'TextBox11
        '
        Me.TextBox11.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.202R), Telerik.Reporting.Drawing.Unit.Cm(2.2R))
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.998R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox11.Value = "=Fields.categoriaMerceologica"
        '
        'TextBox16
        '
        FormattingRule3.Filters.Add(New Telerik.Reporting.Filter("= Fields.PRENOTAZIONE", Telerik.Reporting.FilterOperator.Equal, "0"))
        FormattingRule3.Style.Visible = False
        Me.TextBox16.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule3})
        Me.TextBox16.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.6R), Telerik.Reporting.Drawing.Unit.Cm(0.9R))
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.2R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox16.Value = "='Ordine prenotato per il giorno: '+Fields.DATA_EVASIONE"
        '
        'TextBox17
        '
        FormattingRule4.Filters.Add(New Telerik.Reporting.Filter("Parameters.showDataUltimaModifica.Value", Telerik.Reporting.FilterOperator.NotEqual, "True"))
        FormattingRule4.Style.Visible = False
        Me.TextBox17.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule4})
        Me.TextBox17.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.6R), Telerik.Reporting.Drawing.Unit.Cm(1.5R))
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.2R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox17.Value = "='Ultima modifica del: '+Fields.DATA_ULTIMA_MODIFICA"
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(2.0R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.Table1})
        Me.detail.Name = "detail"
        Me.detail.PageBreak = Telerik.Reporting.PageBreak.None
        '
        'Table1
        '
        Me.Table1.Bindings.Add(New Telerik.Reporting.Binding("DataSource", "= Fields.orderLines"))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(0.521R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(3.081R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(5.23R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1.415R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1.587R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1.155R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1.427R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1.412R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1.035R)))
        Me.Table1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2.136R)))
        Me.Table1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.982R)))
        Me.Table1.Body.SetCellContent(0, 3, Me.TextBox4)
        Me.Table1.Body.SetCellContent(0, 4, Me.TextBox6)
        Me.Table1.Body.SetCellContent(0, 1, Me.TextBox8)
        Me.Table1.Body.SetCellContent(0, 2, Me.TextBox2)
        Me.Table1.Body.SetCellContent(0, 5, Me.TextBox13)
        Me.Table1.Body.SetCellContent(0, 6, Me.TextBox28)
        Me.Table1.Body.SetCellContent(0, 7, Me.TextBox31)
        Me.Table1.Body.SetCellContent(0, 8, Me.TextBox34)
        Me.Table1.Body.SetCellContent(0, 9, Me.TextBox37)
        Me.Table1.Body.SetCellContent(0, 0, Me.CheckBox1)
        TableGroup5.Name = "group3"
        TableGroup5.ReportItem = Me.TextBox18
        TableGroup6.Name = "tableGroup"
        TableGroup6.ReportItem = Me.TextBox1
        TableGroup7.Name = "group"
        TableGroup7.ReportItem = Me.TextBox9
        TableGroup8.Name = "tableGroup1"
        TableGroup8.ReportItem = Me.TextBox3
        TableGroup9.Name = "tableGroup2"
        TableGroup9.ReportItem = Me.TextBox5
        TableGroup10.Name = "group1"
        TableGroup10.ReportItem = Me.TextBox12
        TableGroup11.Name = "group4"
        TableGroup11.ReportItem = Me.TextBox27
        TableGroup12.Name = "group5"
        TableGroup12.ReportItem = Me.TextBox30
        TableGroup13.Name = "group6"
        TableGroup13.ReportItem = Me.TextBox33
        TableGroup14.Name = "group7"
        TableGroup14.ReportItem = Me.TextBox36
        Me.Table1.ColumnGroups.Add(TableGroup5)
        Me.Table1.ColumnGroups.Add(TableGroup6)
        Me.Table1.ColumnGroups.Add(TableGroup7)
        Me.Table1.ColumnGroups.Add(TableGroup8)
        Me.Table1.ColumnGroups.Add(TableGroup9)
        Me.Table1.ColumnGroups.Add(TableGroup10)
        Me.Table1.ColumnGroups.Add(TableGroup11)
        Me.Table1.ColumnGroups.Add(TableGroup12)
        Me.Table1.ColumnGroups.Add(TableGroup13)
        Me.Table1.ColumnGroups.Add(TableGroup14)
        Me.Table1.ColumnHeadersPrintOnEveryPage = True
        Me.Table1.DataSource = Me.ObjectDataSourceOrdine
        Me.Table1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.CheckBox1, Me.TextBox8, Me.TextBox2, Me.TextBox4, Me.TextBox6, Me.TextBox13, Me.TextBox28, Me.TextBox31, Me.TextBox34, Me.TextBox37, Me.TextBox18, Me.TextBox1, Me.TextBox9, Me.TextBox3, Me.TextBox5, Me.TextBox12, Me.TextBox27, Me.TextBox30, Me.TextBox33, Me.TextBox36})
        Me.Table1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.Table1.Name = "Table1"
        TableGroup16.Name = "group2"
        TableGroup15.ChildGroups.Add(TableGroup16)
        TableGroup15.Groupings.Add(New Telerik.Reporting.Grouping(Nothing))
        TableGroup15.Name = "detailTableGroup"
        Me.Table1.RowGroups.Add(TableGroup15)
        Me.Table1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.0R), Telerik.Reporting.Drawing.Unit.Cm(1.621R))
        Me.Table1.Style.Font.Name = "Tahoma"
        Me.Table1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'TextBox4
        '
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.415R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox4.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox4.Value = "=Formato"
        '
        'TextBox6
        '
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.587R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox6.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox6.Value = "=LotNo"
        '
        'TextBox8
        '
        Me.TextBox8.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.5R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.081R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox8.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox8.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox8.StyleName = ""
        Me.TextBox8.Value = "=ItemCode+' '+Farmadati"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.1R), Telerik.Reporting.Drawing.Unit.Cm(0.3R))
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.23R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox2.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox2.StyleName = ""
        Me.TextBox2.Value = "=Descrizione"
        '
        'TextBox13
        '
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.155R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox13.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox13.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.TextBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox13.StyleName = ""
        Me.TextBox13.Value = "=Quantity"
        '
        'TextBox28
        '
        Me.TextBox28.Name = "TextBox28"
        Me.TextBox28.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.427R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox28.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox28.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox28.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox28.StyleName = ""
        Me.TextBox28.Value = "=UnitPrice"
        '
        'TextBox31
        '
        Me.TextBox31.Name = "TextBox31"
        Me.TextBox31.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.412R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox31.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox31.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox31.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox31.StyleName = ""
        Me.TextBox31.Value = "=ScontoRiga"
        '
        'TextBox34
        '
        Me.TextBox34.Name = "TextBox34"
        Me.TextBox34.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.035R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox34.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox34.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox34.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox34.StyleName = ""
        Me.TextBox34.Value = "=IVA"
        '
        'TextBox37
        '
        Me.TextBox37.Name = "TextBox37"
        Me.TextBox37.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.136R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.TextBox37.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.TextBox37.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox37.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox37.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox37.StyleName = ""
        Me.TextBox37.Value = "=TotaleRiga"
        '
        'CheckBox1
        '
        Me.CheckBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.5R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.521R), Telerik.Reporting.Drawing.Unit.Cm(0.982R))
        Me.CheckBox1.Style.BorderColor.Bottom = System.Drawing.Color.DarkGray
        Me.CheckBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.CheckBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.CheckBox1.StyleName = ""
        Me.CheckBox1.Text = ""
        Me.CheckBox1.Value = "= False"
        '
        'pageFooterSection1
        '
        Me.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3.4R)
        Me.pageFooterSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.TextBox19, Me.TextBox61, Me.TextBox64, Me.TextBox65, Me.TextBox66, Me.Barcode1, Me.TextBox60, Me.TextBox63, Me.Shape4})
        Me.pageFooterSection1.Name = "pageFooterSection1"
        Me.pageFooterSection1.PrintOnFirstPage = True
        Me.pageFooterSection1.Style.Font.Name = "Tahoma"
        Me.pageFooterSection1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'TextBox19
        '
        Me.TextBox19.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.2R), Telerik.Reporting.Drawing.Unit.Cm(2.6R))
        Me.TextBox19.Name = "TextBox19"
        Me.TextBox19.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.8R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox19.Value = "= PageNumber+'/'+ PageCount"
        '
        'TextBox61
        '
        Me.TextBox61.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0.3R))
        Me.TextBox61.Name = "TextBox61"
        Me.TextBox61.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox61.Style.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.TextBox61.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.TextBox61.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox61.Value = "='Questo ordine � stato inserito da '+Fields.OperatorCode+' ed elaborato da '+Fie" &
    "lds.User"
        '
        'TextBox64
        '
        Me.TextBox64.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.0R), Telerik.Reporting.Drawing.Unit.Cm(1.7R))
        Me.TextBox64.Name = "TextBox64"
        Me.TextBox64.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8R), Telerik.Reporting.Drawing.Unit.Cm(1.5R))
        Me.TextBox64.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox64.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.TextBox64.Value = "TIMBRO CONTROLLO"
        '
        'TextBox65
        '
        Me.TextBox65.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.035R), Telerik.Reporting.Drawing.Unit.Cm(1.7R))
        Me.TextBox65.Name = "TextBox65"
        Me.TextBox65.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8R), Telerik.Reporting.Drawing.Unit.Cm(1.5R))
        Me.TextBox65.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox65.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.TextBox65.Value = "TIMBRO FATTURAZIONE"
        '
        'TextBox66
        '
        Me.TextBox66.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.067R), Telerik.Reporting.Drawing.Unit.Cm(1.7R))
        Me.TextBox66.Name = "TextBox66"
        Me.TextBox66.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8R), Telerik.Reporting.Drawing.Unit.Cm(1.5R))
        Me.TextBox66.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox66.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.TextBox66.Value = ""
        '
        'Barcode1
        '
        Me.Barcode1.Encoder = QrCodeEncoder1
        Me.Barcode1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.0R), Telerik.Reporting.Drawing.Unit.Cm(1.7R))
        Me.Barcode1.Name = "Barcode1"
        Me.Barcode1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.5R))
        Me.Barcode1.Stretch = True
        Me.Barcode1.TocText = ""
        Me.Barcode1.Value = "= Fields.QRcode"
        '
        'TextBox60
        '
        Me.TextBox60.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(1.0R))
        Me.TextBox60.Name = "TextBox60"
        Me.TextBox60.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox60.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.TextBox60.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox60.Value = "= Fields.reportFooterDescription"
        '
        'TextBox63
        '
        Me.TextBox63.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(1.7R))
        Me.TextBox63.Name = "TextBox63"
        Me.TextBox63.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8R), Telerik.Reporting.Drawing.Unit.Cm(1.5R))
        Me.TextBox63.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox63.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        Me.TextBox63.Value = "TIMBRO RACCOLTA"
        '
        'Shape4
        '
        Me.Shape4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.Shape4.Name = "Shape4"
        Me.Shape4.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.Shape4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.0R), Telerik.Reporting.Drawing.Unit.Cm(0.3R))
        '
        'TextBox14
        '
        Me.TextBox14.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox14.Style.Font.Bold = True
        Me.TextBox14.Value = "= Fields.metododipagamento"
        '
        'TextBox15
        '
        Me.TextBox15.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox15.Value = "Metodo di pagamento"
        '
        'Shape1
        '
        Me.Shape1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(0R))
        Me.Shape1.Name = "Shape1"
        Me.Shape1.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.Shape1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.0R), Telerik.Reporting.Drawing.Unit.Cm(0.3R))
        '
        'ReportFooterSection1
        '
        Me.ReportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(5.3R)
        Me.ReportFooterSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.TextBox14, Me.TextBox15, Me.Shape1, Me.TextBox20, Me.TextBox21, Me.TextBox22, Me.TextBox23, Me.TextBox24, Me.TextBox25, Me.TextBox26, Me.TextBox29, Me.TextBox32, Me.TextBox35, Me.TextBox38, Me.TextBox47, Me.TextBox48, Me.TextBox49, Me.TextBox52, Me.TextBox54, Me.Shape2, Me.TextBox56, Me.TextBox57, Me.TextBox58, Me.Shape3, Me.TextBox59, Me.TextBox62, Me.TextBox68})
        Me.ReportFooterSection1.Name = "ReportFooterSection1"
        Me.ReportFooterSection1.PrintAtBottom = True
        Me.ReportFooterSection1.Style.Font.Name = "Tahoma"
        Me.ReportFooterSection1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        '
        'TextBox20
        '
        Me.TextBox20.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(1.1R))
        Me.TextBox20.Name = "TextBox20"
        Me.TextBox20.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox20.Value = "Termini di pagamento"
        '
        'TextBox21
        '
        Me.TextBox21.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(1.7R))
        Me.TextBox21.Name = "TextBox21"
        Me.TextBox21.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox21.Value = "Sconto su pagamento"
        '
        'TextBox22
        '
        Me.TextBox22.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(2.301R))
        Me.TextBox22.Name = "TextBox22"
        Me.TextBox22.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox22.Value = "Spese di trasporto"
        '
        'TextBox23
        '
        Me.TextBox23.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(2.901R))
        Me.TextBox23.Name = "TextBox23"
        Me.TextBox23.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox23.Value = "Spese di incasso"
        '
        'TextBox24
        '
        Me.TextBox24.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1R))
        Me.TextBox24.Name = "TextBox24"
        Me.TextBox24.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox24.Style.Font.Bold = True
        Me.TextBox24.Value = "= Fields.terminedipagamento"
        '
        'TextBox25
        '
        Me.TextBox25.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(1.7R))
        Me.TextBox25.Name = "TextBox25"
        Me.TextBox25.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox25.Style.Font.Bold = True
        Me.TextBox25.Value = "= Fields.scontoPagamentoPerc"
        '
        'TextBox26
        '
        Me.TextBox26.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(2.901R))
        Me.TextBox26.Name = "TextBox26"
        Me.TextBox26.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox26.Style.Font.Bold = True
        Me.TextBox26.Value = "= Fields.speseIncasso"
        '
        'TextBox29
        '
        Me.TextBox29.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(2.301R))
        Me.TextBox29.Name = "TextBox29"
        Me.TextBox29.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.6R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox29.Style.Font.Bold = True
        Me.TextBox29.Value = "= Fields.speseTrasporto"
        '
        'TextBox32
        '
        Me.TextBox32.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.864R), Telerik.Reporting.Drawing.Unit.Cm(4.0R))
        Me.TextBox32.Name = "TextBox32"
        Me.TextBox32.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.136R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox32.Style.Font.Bold = True
        Me.TextBox32.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox32.Value = "= Fields.totaleOrdine"
        '
        'TextBox35
        '
        Me.TextBox35.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.864R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.TextBox35.Name = "TextBox35"
        Me.TextBox35.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.136R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox35.Style.Font.Bold = True
        Me.TextBox35.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox35.Value = "= Fields.totaleIMPONIBILE"
        '
        'TextBox38
        '
        Me.TextBox38.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.864R), Telerik.Reporting.Drawing.Unit.Cm(3.101R))
        Me.TextBox38.Name = "TextBox38"
        Me.TextBox38.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.136R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox38.Style.Font.Bold = True
        Me.TextBox38.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox38.Value = "= Fields.valoreVoucher"
        '
        'TextBox47
        '
        Me.TextBox47.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.864R), Telerik.Reporting.Drawing.Unit.Cm(2.501R))
        Me.TextBox47.Name = "TextBox47"
        Me.TextBox47.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.136R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox47.Style.Font.Bold = True
        Me.TextBox47.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox47.Value = "= Fields.totaleIVA"
        '
        'TextBox48
        '
        Me.TextBox48.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.864R), Telerik.Reporting.Drawing.Unit.Cm(1.9R))
        Me.TextBox48.Name = "TextBox48"
        Me.TextBox48.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.136R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox48.Style.Font.Bold = True
        Me.TextBox48.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox48.Value = "= Fields.totaleIMPONIBILEnetto"
        '
        'TextBox49
        '
        Me.TextBox49.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.864R), Telerik.Reporting.Drawing.Unit.Cm(1.1R))
        Me.TextBox49.Name = "TextBox49"
        Me.TextBox49.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.136R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox49.Style.Font.Bold = True
        Me.TextBox49.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox49.Value = "= Fields.scontoTotale"
        '
        'TextBox52
        '
        Me.TextBox52.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.864R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.TextBox52.Name = "TextBox52"
        Me.TextBox52.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox52.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox52.Value = "Totale imponibile"
        '
        'TextBox54
        '
        Me.TextBox54.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.832R), Telerik.Reporting.Drawing.Unit.Cm(1.1R))
        Me.TextBox54.Name = "TextBox54"
        Me.TextBox54.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox54.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox54.Value = "Sconto pagamento"
        '
        'Shape2
        '
        Me.Shape2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.0R), Telerik.Reporting.Drawing.Unit.Cm(1.7R))
        Me.Shape2.Name = "Shape2"
        Me.Shape2.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.Shape2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0R), Telerik.Reporting.Drawing.Unit.Cm(0.132R))
        '
        'TextBox56
        '
        Me.TextBox56.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.957R), Telerik.Reporting.Drawing.Unit.Cm(1.9R))
        Me.TextBox56.Name = "TextBox56"
        Me.TextBox56.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.907R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox56.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox56.Value = "Totale imponibile netto"
        '
        'TextBox57
        '
        Me.TextBox57.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.957R), Telerik.Reporting.Drawing.Unit.Cm(2.501R))
        Me.TextBox57.Name = "TextBox57"
        Me.TextBox57.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.907R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox57.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox57.Value = "Totale iva"
        '
        'TextBox58
        '
        Me.TextBox58.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.957R), Telerik.Reporting.Drawing.Unit.Cm(3.101R))
        Me.TextBox58.Name = "TextBox58"
        Me.TextBox58.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.907R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox58.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox58.Value = "Sconto voucher"
        '
        'Shape3
        '
        Me.Shape3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.0R), Telerik.Reporting.Drawing.Unit.Cm(3.701R))
        Me.Shape3.Name = "Shape3"
        Me.Shape3.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.Shape3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0R), Telerik.Reporting.Drawing.Unit.Cm(0.132R))
        '
        'TextBox59
        '
        Me.TextBox59.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.925R), Telerik.Reporting.Drawing.Unit.Cm(4.0R))
        Me.TextBox59.Name = "TextBox59"
        Me.TextBox59.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.907R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox59.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.TextBox59.Value = "Totale ordine"
        '
        'TextBox62
        '
        FormattingRule5.Filters.Add(New Telerik.Reporting.Filter("= Fields.Notes Is Null Or Fields.Notes Like ''", Telerik.Reporting.FilterOperator.Equal, "= True"))
        FormattingRule5.Style.Visible = False
        Me.TextBox62.ConditionalFormatting.AddRange(New Telerik.Reporting.Drawing.FormattingRule() {FormattingRule5})
        Me.TextBox62.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0R), Telerik.Reporting.Drawing.Unit.Cm(4.7R))
        Me.TextBox62.Name = "TextBox62"
        Me.TextBox62.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.0R), Telerik.Reporting.Drawing.Unit.Cm(0.6R))
        Me.TextBox62.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox62.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.TextBox62.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox62.Value = "= 'Note: ' +Fields.Notes"
        '
        'ObjectDataSourceOrdine
        '
        Me.ObjectDataSourceOrdine.DataMember = "getOrder"
        Me.ObjectDataSourceOrdine.DataSource = GetType(servisWO.dataReportManager)
        Me.ObjectDataSourceOrdine.Name = "ObjectDataSourceOrdine"
        Me.ObjectDataSourceOrdine.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("ordercode", GetType(String), "= Parameters.orderCode.Value"))
        Me.ObjectDataSourceOrdine.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("NAVconnectionString", GetType(String), "= Parameters.NAVconnectionString.Value"))
        Me.ObjectDataSourceOrdine.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("WORconnectionString", GetType(String), "= Parameters.WORconnectionString.Value"))
        Me.ObjectDataSourceOrdine.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("workingCompany", GetType(String), "= Parameters.workingCompany.Value"))
        Me.ObjectDataSourceOrdine.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("showDataUltimaModifica", GetType(Boolean), "= Parameters.showDataUltimaModifica.Value"))
        Me.ObjectDataSourceOrdine.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("linesOrderByField", GetType(String), "= Parameters.linesOrderByField.Value"))
        Me.ObjectDataSourceOrdine.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("reportLogo", GetType(String), "= Parameters.reportLogo.Value"))
        Me.ObjectDataSourceOrdine.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("reportFooterDescription", GetType(String), "= Parameters.reportFooterDescription.Value"))
        '
        'Report1
        '
        Me.DataSource = Me.ObjectDataSourceOrdine
        Group1.GroupFooter = Me.groupFooterSection
        Group1.GroupHeader = Me.groupHeaderSection
        Group1.Name = "group8"
        Me.Groups.AddRange(New Telerik.Reporting.Group() {Group1})
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.groupHeaderSection, Me.groupFooterSection, Me.pageHeaderSection1, Me.detail, Me.pageFooterSection1, Me.ReportFooterSection1})
        Me.Name = "Report1"
        Me.PageSettings.ContinuousPaper = False
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(10.0R), Telerik.Reporting.Drawing.Unit.Mm(10.0R), Telerik.Reporting.Drawing.Unit.Mm(10.0R), Telerik.Reporting.Drawing.Unit.Mm(10.0R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        ReportParameter1.Name = "orderCode"
        ReportParameter1.Value = "500350"
        ReportParameter2.Name = "NAVconnectionString"
        ReportParameter2.Value = "Data Source=10.10.101.3;Initial Catalog=DRGIORGINICOLL;Persist Security Info=True" &
    ";User ID=giorgini;Password=giorgini"
        ReportParameter3.Name = "WORconnectionString"
        ReportParameter3.Value = "Data Source=10.10.101.3;Initial Catalog=DR-GIORGINI-WEBORDER-V4;Persist Security " &
    "Info=True;User ID=giorgini;Password=giorgini"
        ReportParameter4.Name = "workingCompany"
        ReportParameter4.Value = "Ser-Vis Srl"
        ReportParameter5.Name = "showDataUltimaModifica"
        ReportParameter5.Type = Telerik.Reporting.ReportParameterType.[Boolean]
        ReportParameter5.Value = "True"
        ReportParameter6.Name = "linesOrderByField"
        ReportParameter6.Value = "LineID"
        ReportParameter7.Name = "reportLogo"
        ReportParameter7.Value = "~/images/custom/logo_servis.png"
        ReportParameter8.Name = "reportFooterDescription"
        ReportParameter8.Value = "Ufficio Ordini: Ser-Vis S.r.l. Via Isonzo, 67 40033 - Casalecchio di Reno (BO) - " &
    "email: ordini@drgiorgini.com - Tel 800.180.631 - Fax 800.910.329"
        Me.ReportParameters.Add(ReportParameter1)
        Me.ReportParameters.Add(ReportParameter2)
        Me.ReportParameters.Add(ReportParameter3)
        Me.ReportParameters.Add(ReportParameter4)
        Me.ReportParameters.Add(ReportParameter5)
        Me.ReportParameters.Add(ReportParameter6)
        Me.ReportParameters.Add(ReportParameter7)
        Me.ReportParameters.Add(ReportParameter8)
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1})
        Me.Width = Telerik.Reporting.Drawing.Unit.Cm(19.0R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents pageHeaderSection1 As Telerik.Reporting.PageHeaderSection
    Friend WithEvents detail As Telerik.Reporting.DetailSection
    Friend WithEvents pageFooterSection1 As Telerik.Reporting.PageFooterSection
    Friend WithEvents ObjectDataSourceOrdine As Telerik.Reporting.ObjectDataSource
    Friend WithEvents Table1 As Telerik.Reporting.Table
    Friend WithEvents TextBox4 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox6 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox1 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox3 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox5 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox7 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox8 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox2 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox9 As Telerik.Reporting.TextBox
    Friend WithEvents PictureBox1 As Telerik.Reporting.PictureBox
    Friend WithEvents TextBox13 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox12 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox14 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox15 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox28 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox31 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox34 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox37 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox27 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox30 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox33 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox36 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox10 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox40 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox41 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox42 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox43 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox44 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox46 As Telerik.Reporting.TextBox
    Friend WithEvents Shape1 As Telerik.Reporting.Shape
    Friend WithEvents CheckBox1 As Telerik.Reporting.CheckBox
    Friend WithEvents TextBox18 As Telerik.Reporting.TextBox
    Friend WithEvents ReportFooterSection1 As Telerik.Reporting.ReportFooterSection
    Friend WithEvents TextBox19 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox20 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox21 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox22 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox23 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox24 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox25 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox26 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox29 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox32 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox35 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox38 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox47 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox48 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox49 As Telerik.Reporting.TextBox
    Friend WithEvents groupHeaderSection As Telerik.Reporting.GroupHeaderSection
    Friend WithEvents groupFooterSection As Telerik.Reporting.GroupFooterSection
    Friend WithEvents Table2 As Telerik.Reporting.Table
    Friend WithEvents TextBox51 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox53 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox55 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox50 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox52 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox54 As Telerik.Reporting.TextBox
    Friend WithEvents Shape2 As Telerik.Reporting.Shape
    Friend WithEvents TextBox56 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox57 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox58 As Telerik.Reporting.TextBox
    Friend WithEvents Shape3 As Telerik.Reporting.Shape
    Friend WithEvents TextBox59 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox60 As Telerik.Reporting.TextBox
    Friend WithEvents Shape4 As Telerik.Reporting.Shape
    Friend WithEvents TextBox61 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox62 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox63 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox64 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox65 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox66 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox67 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox45 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox11 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox16 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox17 As Telerik.Reporting.TextBox
    Friend WithEvents Barcode1 As Telerik.Reporting.Barcode
    Friend WithEvents TextBox68 As Telerik.Reporting.TextBox


End Class