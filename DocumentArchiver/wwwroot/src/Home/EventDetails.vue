<template id="event-details-template">
    <td class="borderless" colspan="9999">
        <div class="card shadow-nohover">
            <div class="card-body">
                <table class="table table-sm borderless">
                    <thead>
                        <tr class="text-danger">
                            <td>#</td>
                            <td>Tên</td>
                            <td>Ngày</td>
                            <td>Tạo Ngày</td>
                            <td>Loại file</td>
                            <td>Người đăng</td>
                            <td>Note</td>
                            <!--<td>
                                <button class="btn btn-sm">
                                    <span class="fa fa-plus"></span>
                                </button>
                            </td>-->
                        </tr>
                    </thead>
                    <tr v-for="event in events">
                        <td class="top-border"><span>{{event.EventId}}</span></td>
                        <td class="top-border"><span>{{event.Name}}</span></td>
                        <td class="top-border"><span>{{event.DateOfEvent}}</span></td>
                        <td class="top-border"><span>{{event.CreateTime}}</span></td>
                        <td class="top-border"><span>{{event.Filetype}}</span></td>
                        <td class="top-border"><span>{{event.Username}}</span></td>
                        <td class="top-border"><div class="wrap-text">{{event.Note}}</div></td>
                        <!--<td><button class="btn-link"><span class="fa fa-pencil-square-o"></span></button></td>-->
                    </tr>
                </table>
            </div>
        </div>
    </td>
</template>
<script>
    export default {
        name: 'event-details',
        template: '#event-details-template',
        props: ['events']
    }
</script>
<style scoped>
    table.borderless td {
        border: none;
    }
    .wrap-text{
        word-wrap: break-word;
    }
    .top-border {
        border-top: 1px solid #dee2e6 !important;
    }
    .shadow-nohover {
        box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
    }
</style>