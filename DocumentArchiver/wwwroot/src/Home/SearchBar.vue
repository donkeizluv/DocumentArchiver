<template id="search-bar-template">
    <div class="form-inline">
        <div class="form-group">
            <select v-model="SearchModel.FilterBy" class="form-control custom-select" v-bind:disabled="isdisabled">
                <option value="ContractNumber">Số hợp đồng</option>
                <option value="CustomerName">Tên khách hàng</option>
                <option value="IdentityCard">CMND</option>
                <option value="Phone">SDT</option>
                <option value="Username">Người tạo</option>
            </select>
            <input v-on:keyup.enter="SearchButtonClicked" v-model="SearchModel.FilterString" type="text" class="form-control" placeholder="Từ khóa..." v-bind:disabled="isdisabled"/>
            <button v-on:click="SearchButtonClicked" type="button" class="btn btn-link" v-bind:disabled="isdisabled"><span class="fa fa-search" aria-hidden="true"></span></button>
        </div>
    </div>
</template>
<script>
    export default {
        name: 'search-bar',
        template: '#search-bar-template',
        props: ['isdisabled'],
        data: function () {
            return {
                SearchModel: {
                    FilterBy: 'ContractNumber',
                    FilterString: ""
                }
            };
        },
        methods: {
            SearchButtonClicked: function () {
                this.$emit('submit', this.$data.SearchModel);
            }
        }
    }
</script>