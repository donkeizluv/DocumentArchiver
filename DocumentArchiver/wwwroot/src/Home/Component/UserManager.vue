<template id="user-manager">
    <div>
        <v-dialog :clickToClose=false />
        <div class="row top-margin">
            <div class="col-sm-8 mx-auto">
                <search-bar v-bind:isdisabled="IsLoading"
                            v-bind:value-pairs="SearchBarValues"
                            v-on:submit="SearchButtonClicked"></search-bar>
            </div>
        </div>
        <div class="row top-margin">
            <div class="col-12">
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th>
                                    Username
                                </th>
                                <th>
                                    Active
                                </th>
                                <th>
                                    Last login
                                </th>
                                <th>
                                    Abilities
                                </th>
                                <th>
                                    Layer
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="item in Items" v-bind:key="item.Username">
                                <td>
                                    {{item.Username}}
                                </td>
                                <td>
                                    {{item.Active}}
                                </td>
                                <td>
                                    {{item.LastLogin}}
                                </td>
                                <td>
                                    {{item.Ablities}}
                                </td>
                                <td>
                                    {{item.LayerName}}
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="row top-margin">
            <div class="col-lg-6 mx-auto">
                <page-nav :page-count="TotalPages"
                          :click-handler="PageNavClicked"
                          :page-range="2"
                          :prev-text="'Trước'"
                          :force-page="OnPage - 1"
                          :next-text="'Sau'"
                          :page-class="'page-item'"
                          :page-link-class="'page-link'"
                          :prev-class="'page-item'"
                          :prev-link-class="'page-link'"
                          :next-class="'page-item'"
                          :next-link-class="'page-link'"
                          :container-class="'pagination pagination-sm justify-content-center'">
                </page-nav>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios'
    import pagenav from 'vuejs-paginate'
    import queryBuilder from 'query-string'
    import searchBar from './SearchBar.vue'

    export default {
        name: 'UserManager',
        template: '#user-manager',
        components: {
            'search-bar': searchBar,
            'page-nav': pagenav
        },
        created: function () {
            this.Init();
        },
        computed: {

        },
        data: function () {
            return {
                Loading: false,

                OnPage: 1,
                FilterString: '',
                //Injected: null,
                Items: [],
                TotalRows: 0,
                TotalPages: 0,

                //Crud stuff

                //Search bar
                SearchBarValues: [{ value: 'Username', name: 'Username' }]
            }
        },
        methods: {
            Init: function () {
                //Load injected
                var model = window.model;
                if (!model) {
                    //Show app init failed
                    this.ShowErrorDialog(GeneralError);
                    return;
                }
                this.FilterString = model.FilterString;
                this.OnPage = model.OnPage;
                this.Items = model.Items;
                this.UpdatePagination(model.TotalPages, model.TotalRows);
            },
            //update paging
            UpdatePagination: function (totalPages, totalRows) {
                this.TotalPages = totalPages;
                this.TotalRows = totalRows;
            },
            ShowErrorDialog(mess) {
                this.$modal.show('dialog', {
                    title: 'Lỗi :(',
                    text: mess,
                    buttons: [
                        {
                            title: 'Đóng',
                            handler: () => { this.$modal.hide('dialog') }
                        }
                    ]
                });
            }
        }
    }
</script>
<style scoped>

</style>