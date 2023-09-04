var app = new Vue({
    el:"#content",
    data:{
        loder_img: false,
       
    },
    methods: {
       
        mommentDate: function (date) {
            return moment(date).format('DD/MM/YYYY LT');
        },
    
        isNormalPositiveInteger: function (str) {
            return /^\+?([1-9]\d*)$/.test(str);
        },
        isNormalPositiveFloting: function (str) {
            if (str == 0) return false
            return /^\+?([1-9]\d*|([+]?[0-9]*\.[0-9]+|[0-9]+))$/.test(str);
        },
       
        tableData: function () {
            this.tblData = $("#example").DataTable({
                //stateSave: true, // same view in page lode
                "processing": true, // for show progress bar    
                "serverSide": true, // for process server side    
                "filter": true, // this is for disable filter (search box)  
                "paging": true,
                "lengthMenu": [20, 30, 50, 100, 500],
                "responsive": true,
              //  "order": [[1, 'asc']], // default ordering
                "ajax": {
                    "url": '/Category/LoadData',
                    "datatype": "json",
                    "type": "POST",
                    "data": {
                        menu_type: this.menu_type
                    },
                },
                "columnDefs": [
                // { "targets": 0, "orderable": false, visible: false },
                    { "targets": 0, "className": "text-center", },
                    { "targets": 1, "className": "text-center", },
                    { "targets": 2, "className": "text-center", },
                    { "targets": 3, "className": "text-center", },
                    { "targets": 4, "className": "text-center", },
                    { "targets": 5, "className": "text-center", },
                ],
                "columns": [
                    {
                        "data": "rowNo",
                        "render": function (data, type, full, meta) {
                            return meta.row + 1;
                        },
                        "name": "Id"
                    },
                    { "data": "name", "name": "name" },
                    { "data": "parentName", "name": "parentName" },
                    { "data": "createdByName", "name": "CreatedByName" },
                    {
                        "data": "createdAt",
                        "render": function (data, type, full, meta) {
                            return app.mommentDate(data);
                        },
                        "name": "CreatedAt"
                    },
                    {
                        "data": "Id",
                        "render": function (data, type, full, meta) {
                            return "";
                        },
                        "name": "Id"
                    },
                ]

            });
        },

      
      toFormData: function(obj){
        var form_data = new FormData();
       // form_data.append('_token', $('meta[name=csrf-token]').attr('content'));
        for (var key in obj) {
            form_data.append(key, (obj[key] == null) ? '' : obj[key]);
        }
        return form_data;
     }
    },
    mounted: function () {
        this.tableData()
        console.log("Succefully Implement"); // for succesfully implement or not 
    }
})


