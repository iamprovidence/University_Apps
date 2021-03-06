<!DOCTYPE html>
<html lang="en">
<head>
    <?php include_once (ROOT ."/view/moderator/layout/head.php"); ?> 
</head>
<body>
    <!--HEADER-->
    <?php include_once (ROOT ."/view/moderator/layout/header.php"); ?>      
    
    <!--MENU-->
    <?php include_once (ROOT ."/view/moderator/layout/menu.php"); ?>   
    
    <!--CONTENT-->
    <div id="main" class="container-fluid">
      
        <!--TABLE-->
        <h3>Messages</h3>
        <div id="table-data" >
            <table class="table table-striped">

                <thead>
                    <tr>
                        <th scope="col">№</th>
                        <th scope="col">Subject</th>
                        <th scope="col">Email</th>
                        <th scope="col">Date</th>
                        <th scope="col" class="text-center">Read</th>
                        <th scope="col" class="text-center">Delete</th>
                    </tr>
                </thead>

                <tbody>                   
                    <?php foreach($messages as $message): ?>
                    
                    <tr>                       
                        <th scope="row"><?=$message->id?></th>
                        <td><?=$message->subject_name?></td>
                        <td><?=$message->user_email?></td>
                        <td><?=$message->date?></td>
                        
                        <td class="w-50-px">
                            <a href="/moderator/messages/<?=$message->id?>/" class="btn btn-warning btn-read">Read</a>
                        </td>
                        <td class="w-50-px">
                            <button type="button" data-id="<?=$message->id?>" class="btn btn-danger btn-delete">Delete</button>
                        </td>
                    </tr>                       
                    <?php endforeach; ?>
                    
                </tbody>
            </table>
        </div>
        
        <!--PAGINATION-->
        <?php echo $pagination->get(); ?>          

    </div>
    
     
    <!--SCRIPTS-->
    <?php include_once (ROOT ."/view/moderator/layout/scripts.php"); ?>    
    <script>
   
    //delete
    function bind_deleting()
    {    
        $("button.btn-delete").click(function()
        {
            const entity_id = $(this).data("id");
            const page_number = $('ul.pagination li.active a').text();
            
            notie.confirm(
            {
                text: 'Are you sure?',
                submitCallback: function()
                {        
                    // confirm deleting                
                    $.post("/moderator/messages/delete/",
                    {
                        entity_id:  entity_id,
                        submit:     true
                    },
                    function(operationStatus)
                    {
                        operationStatus = JSON.parse(operationStatus);

                        notie.alert(
                        {
                            type: operationStatus.isOperationSucceeded ? "success" : "error", 
                            text: operationStatus.message
                        }); 

                        if (operationStatus.isOperationSucceeded) load_view({entity_name: 'messages', page_number: page_number});
                        
                    });
                }
            });        
        });
    }
    </script>

</body>
</html>