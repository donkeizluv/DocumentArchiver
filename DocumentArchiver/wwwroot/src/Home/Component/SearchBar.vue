<template id="search-bar-template">
    <div class="form-inline justify-content-center">
        <div class="form-group">
            <select v-model="SearchModel.FilterBy" class="form-control custom-select" v-bind:disabled="isdisabled">
                <option value="ContractNumber">Số hợp đồng</option>
                <option value="CustomerName">Tên khách hàng</option>
                <option value="IdentityCard">CMND</option>
                <option value="Phone">SDT</option>
                <option value="Username">Người tạo</option>
            </select>
            <div class="input-group">
                <input v-on:keyup.enter="SearchButtonClicked"
                           v-model="SearchModel.FilterString"
                           class="form-control"
                           placeholder="Từ khóa..."
                           v-bind:disabled="isdisabled" 
                           type="search">
                <span class="input-group-btn">
                    <button class="btn btn-link"
                            type="button"
                            v-on:click="SearchButtonClicked">
                        <i class="fa fa-search"></i>
                    </button>
                </span>
            </div>
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
                    FilterString: ''
                }
            };
        },
        methods: {
            SearchButtonClicked: function () {
                if (this.isdisabled) return;
                this.$emit('submit', this.$data.SearchModel);
            }
        }
    }
</script>
<style scoped>
    .no-right-pad{
        padding-right: 0;
    }
    .no-padding{
        padding: 0;
    }
</style>