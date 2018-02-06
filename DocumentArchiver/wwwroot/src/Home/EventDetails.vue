<template id="event-details-template">
    <td class="borderless" v-bind:colspan="spancol">
        <div class="row justify-content-end">
            <div class="col-12">
                <div class="card shadow-nohover">
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-sm borderless">
                                <thead>
                                    <tr>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('EventId')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('EventId')"></span>#
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Name')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Name')"></span>Sự kiện
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('DateOfEvent')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('DateOfEvent')"></span>Ngày sự kiện
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('CreateTime')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('CreateTime')"></span>Ngày tạo
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Filetype')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Filetype')"></span>Loại file
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Username')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Username')"></span>Người đăng
                                            </button>
                                        </td>
                                        <td>
                                            <span class="d-lg-inline-flex">Ghi chú</span>
                                        </td>
                                        <td>
                                            <div class="d-inline-flex">
                                                <button class="btn btn-link"
                                                        v-on:click="DownloadZip">
                                                    <span class="fas fa-download" />
                                                    Tất cả
                                                </button>
                                            </div>
                                        </td>
                                        <td>
                                            <span>Chỉnh sửa</span>
                                        </td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="event in Items">
                                        <td class="top-border"><span>{{event.EventId}}</span></td>
                                        <!--Name-->
                                        <td v-if="IsEditMode(event.EventId)" class="top-border">
                                            <input type="text"
                                                   class="form-control form-control-sm" 
                                                   v-model="event.Name" 
                                                   v-bind:maxlength="FieldLength.Name"/>
                                        </td>
                                        <td v-else class="top-border">
                                            <span>{{event.Name}}</span>
                                        </td>
                                        <!--Date of event-->
                                        <td v-if="IsEditMode(event.EventId)" class="top-border">
                                            <input type="date" class="form-control form-control-sm" v-model="event.DateOfEventJS" />
                                        </td>
                                        <td v-else class="top-border">
                                            <span>{{event.DateOfEventString}}</span>
                                        </td>
                                        <td class="top-border"><span>{{event.CreateTimeString}}</span></td>
                                        <td class="top-border"><span>{{event.Filetype}}</span></td>
                                        <td class="top-border"><span>{{event.Username}}</span></td>
                                        <!--Note-->
                                        <td v-if="IsEditMode(event.EventId)" class="top-border">
                                            <input type="text"
                                                   class="form-control form-control-sm"
                                                   v-model="event.Note"
                                                   v-bind:maxlength="FieldLength.Note"/>
                                        </td>
                                        <td v-else class="top-border">
                                            <span>{{event.Note}}</span>
                                        </td>
                                        <!--Download file-->
                                        <td class="top-border">
                                            <button class="btn btn-sm btn-link"
                                                    v-on:click="DownloadFile(event.EventId)">
                                                <span class="fas fa-download"/>
                                            </button>
                                        </td>
                                        <!--CRUD-->
                                        <td class="top-border">
                                            <div class="d-inline-flex">
                                                <!--Clear changes button-->
                                                <button v-if="IsEditMode(event.EventId)"
                                                        class="btn btn-sm btn-warning"
                                                        v-on:click="ExitEditMode(event.EventId)">
                                                    <span class="fas fa-times"></span>
                                                </button>
                                                <!--Enter edit-->
                                                <button v-else
                                                        class="btn btn-sm btn-outline-primary"
                                                        v-on:click="EnterEditMode(event.EventId)">
                                                    <span class="fas fa-pencil-alt"></span>
                                                </button>
                                                <!--Save changes-->
                                                <button class="btn btn-sm mr-2 ml-2"
                                                        v-bind:class="{'btn-outline-success': CanSaveItem(event.EventId),
                                                        'btn-outline-secondary': !CanSaveItem(event.EventId)}"
                                                        v-bind:disabled="!CanSaveItem(event.EventId)"
                                                        v-on:click="SubmitChanges(event.EventId)">
                                                    <span class="fas fa-save"></span>
                                                </button>
                                                <!--Delete-->
                                                <button v-if="!IsDeleteMode(event.EventId)"
                                                        class="btn btn-sm"
                                                        v-bind:disabled="IsEditMode(event.EventId)"
                                                        v-bind:class="{'btn-outline-secondary': IsEditMode(event.EventId),
                                                        'btn-outline-danger': !IsEditMode(event.EventId)}"
                                                        v-on:click="EnterDeleteMode(event.EventId)">
                                                    <span class="fas fa-trash-alt"></span>
                                                </button>
                                                <button v-else
                                                        class="btn btn-sm btn-outline-danger"
                                                        v-on:click="DeleteEvent(event.EventId)">
                                                   <span class="fas fa-check-circle"></span>
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                    <!--Add new record-->
                                    <tr>
                                        <td class="top-border"><span class="text-primary form-text">*</span></td>
                                        <td class="top-border">
                                            <input type="text"
                                                   class="form-control form-control-sm"
                                                   v-bind:class="{'attention-border' : !IsNewEventNameValid, 'green-border' : IsNewEventNameValid}"
                                                   v-model="NewItem.Name"
                                                   v-bind:maxlength="FieldLength.Name"/>
                                        </td>
                                        <td class="top-border">
                                            <input type="date"
                                                   class="form-control form-control-sm"
                                                   v-bind:class="{'attention-border' : !IsNewEventDateValid, 'green-border' : IsNewEventDateValid}"
                                                   v-model="NewItem.DateOfEvent"/>
                                        </td>
                                        <td class="top-border" colspan="2">
                                            <uploader v-bind:accept="UploaderOption.Accept"
                                                      v-bind:max-size="UploaderOption.MaxSize"
                                                      button-text="Upload (3mb<)"
                                                      ref="uploader">
                                            </uploader>
                                        </td>
                                        <!--Upload-->
                                        <!--<td class="top-border">
                                            <uploader v-bind:accept="UploaderOption.Accept"
                                                      v-bind:max-size="UploaderOption.MaxSize"
                                                      button-text="Upload (3mb<)"
                                                      ref="uploader">
                                            </uploader>
                                        </td>-->
                                        <td class="top-border" colspan="3">
                                            <input type="text" 
                                                   v-model="NewItem.Note" 
                                                   class="form-control form-control-sm"
                                                   v-bind:maxlength="FieldLength.Note"/>
                                        </td>
                                        <!--<td class="top-border">
                                            <input type="text" v-model="NewItem.Note" class="form-control"/>
                                        </td>-->
                                        <!--Empty download-->
                                        <td class="top-border">
                                            <button v-on:click="PostNewItem"
                                                    v-bind:disabled="!CanSaveNewItem"
                                                    class="btn btn-sm"
                                                    v-bind:class="{'btn-success' : CanSaveNewItem,
                                                    'btn-secondary' : !CanSaveNewItem}">
                                                OK
                                            </button>
                                        </td>
                                        <!--<td class="top-border">
                                            If no errors then enable save new btn
                                            <button v-on:click="PostNewItem"
                                                    v-bind:disabled="!CanSaveNewItem"
                                                    class="btn btn-sm mt-1"
                                                    v-bind:class="{'btn-success' : CanSaveNewItem,
                                                    'btn-secondary' : !CanSaveNewItem}">
                                                OK
                                            </button>
                                        </td>-->
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <page-nav v-show="CanShowPageNav"
                                    :page-count="TotalPages"
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
                                    :container-class="'pagination pagination-sm no-bottom-margin justify-content-center'">
                        </page-nav>
                    </div>
                </div>
            </div>
        </div>
    </td>
</template>
<script>
    import API from '../Home/API'
    import axios from 'axios'
    import pagenav from 'vuejs-paginate'
    import queryBuilder from 'query-string'
    import uploader from '../Home/Uploader.vue'
    import moment from 'moment'

    const JSDate = 'YYYY-MM-DD';
    const successEvent = 'success';
    const exEvent = 'exception';

    export default {
        name: 'event-details',
        template: '#event-details-template',
        components: {
            'page-nav': pagenav,
            'uploader': uploader
        },
        props: {
            id: Number,
            spancol: {
                type: Number,
                default: 7
            }
        },
        data: function () {
            return {
                Loading: false,
                Populated: false,

                OnPage: 1,
                OrderBy: 'EventId',
                OrderAsc: true,

                NewItem: {
                    Name: null,
                    //DateOfEvent: moment().format(JSDate),
                    DateOfEvent: null,
                    Note: null
                },
                FieldLength: {
                    Name: 50,
                    Note: 150,
                },
                Items: [],
                ItemsCopy: [], //Store original item to support revert
                TotalRows: 0,
                TotalPages: 0,

                UploaderOption: {
                    Accept: '.jpg, .jpeg, .bmp, .png, .doc, .docx, .msg, .pdf',
                    MaxSize: 3145728 //3 MB
                }
                
            };
        },
        computed: {
            IsNewEventDateValid: function () {
                return this.IsDateValid(this.$data.NewItem.DateOfEvent);
            },
            IsNewEventFileValid: function () {
                return this.$refs.uploader.IsFileValid;
            },
            IsNewEventNameValid: function () {
                if (this.$data.NewItem.Name)
                    return true;
                return false;
            },
            CanSaveNewItem: function () {
                if (this.IsNewEventNameValid &&
                    this.IsNewEventDateValid &&
                    this.IsNewEventFileValid)
                    return true;
                return false;
            },
            //Only show page nav if total pages > 1
            CanShowPageNav: function () {
                return this.$data.TotalPages > 1;
            },
            GetCurrentItemsAPI: function () {
               return API.EventListingAPI + queryBuilder.stringify(this.ComposeCurrentItemsQuery(this.$data.OnPage));
            },
        },
        created: function () {
            //Listen to parent's event
            this.$parent.$on('populatedetails', this.Populate);
        },
        methods: {
            ComposeCurrentItemsQuery: function (pageNumber) {
                return {
                    id: this.id,
                    page: pageNumber,
                    order: this.$data.OrderBy,
                    asc: this.$data.OrderAsc,
                };
            },
            Populate: function (id) {
                //Check if this component being clicked
                if (id != this.id) return;
                //Check if has items
                if (this.$data.Populated) {
                    //Do nothing
                    this.$emit('populatecompleted');
                    return;
                }
                //Load items
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            LoadItems: function (url) {
                //console.log(url);
                this.$data.Loading = true;
                var that = this;

                axios.get(url)
                    .then(function (response) {
                        if (response.headers.login) {
                            window.location.href = response.headers.login;
                        }
                        that.$data.Items = response.data.Items;
                        that.RefreshCopy(); //Clone Items to arr
                        that.UpdatePagination(response.data.TotalPages, response.data.TotalRows);
                        that.$data.Loading = false;
                        that.$data.Populated = true;
                        that.$emit('populatecompleted');
                    })
                    .catch(function (error) {
                        console.log(error);
                        that.$emit(exEvent, error);
                    });
            },

            PostNewItem: function () {
                var url = API.NewEventAPI;
                var that = this;
                var formData = new FormData();
                formData.append('ContractId', this.id);
                formData.append('Name', this.$data.NewItem.Name);
                formData.append('DateOfEvent', this.$data.NewItem.DateOfEvent);
                formData.append('File', this.$refs.uploader.GetFile);
                formData.append('Note', this.$data.NewItem.Note);
                //console.log(formData);

                axios.post(url, formData)
                    .then(function (response) {
                        if (response.headers.login) {
                            //Login expired -> Redirect
                            window.location.href = response.headers.login;
                            return;
                        }
                        that.$emit(successEvent, 'Thêm sự kiện mới thành công!')
                        that.ClearNewItem();
                        that.Refresh();
                    })
                    .catch(function (error) {
                        //Not 2xx code
                        console.log(error);
                        that.$emit(exEvent, 'Lỗi trong quá trình tạo sự kiện mới.');
                    });
            },
            SubmitChanges: function (id) {
                var url = API.UpdateEventAPI;
                var that = this;
                var itemIndex = this.FindItemIndex(id)
                var formData = new FormData();
                formData.append('EventId', id);
                formData.append('Name', this.$data.Items[itemIndex].Name);
                formData.append('DateOfEvent', this.$data.Items[itemIndex].DateOfEventJS);
                formData.append('Note', this.$data.Items[itemIndex].Note);
                //console.log(formData);

                axios.post(url, formData)
                    .then(function (response) {
                        if (response.headers.login) {
                            //Login expired -> Redirect
                            window.location.href = response.headers.login;
                            return;
                        }
                        that.$emit(successEvent, 'Cập nhật thành công!');
                        that.$set(that.$data.Items[itemIndex], 'EditMode', false)
                        that.Refresh();
                    })
                    .catch(function (error) {
                        console.log(error);
                        that.$emit(exEvent, 'Lỗi trong quá trình cập nhật sự kiên.');
                    });
            },
            DeleteEvent: function (id) {
                var url = API.DeleteEventAPI;
                var that = this;
                var itemIndex = this.FindItemIndex(id)
                var formData = new FormData();
                formData.append('eventId', id);
                //console.log(formData);

                axios.post(url, formData)
                    .then(function (response) {
                        if (response.headers.login) {
                            //Login expired -> Redirect
                            window.location.href = response.headers.login;
                            return;
                        }
                        that.$emit(successEvent, 'Xóa thành công!');
                        that.Refresh();
                    })
                    .catch(function (error) {
                        console.log(error);
                        that.$emit(exEvent, 'Lỗi trong quá trình xóa sự kiên.');
                    });
            },
            CanSaveItem(id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('CanSaveItem: Cant find ' + id);
                    return false;
                }
                //Must be in Edit mode to save
                if (!this.IsEditMode(id)) return false;
                //Values check
                if (this.$data.Items[index].Name &&
                    this.IsDateValid(this.$data.Items[index].DateOfEventJS))
                    return true;
                return false;
            },
            RefreshCopy: function () {
                var i = this.$data.Items.length;
                //No better way to clone?
                while (i--) this.$data.ItemsCopy[i] = JSON.parse(JSON.stringify(this.$data.Items[i]));
            },
            ExitEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('ExitEditMode: Cant find ' + id);
                    return;
                }
                //No better way to clone?
                var revert = JSON.parse(JSON.stringify(this.$data.ItemsCopy[index]));
                //console.log(clone);
                this.$set(this.$data.Items, index, revert);
            },
            //Values changed tracking
            //Edit mode
            EnterEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('EnterEditMode: Cant find ' + id);
                    return;
                }
                //Exit delete mode if needed
                this.$set(this.$data.Items[index], 'DeletePromt', false)
                this.$set(this.$data.Items[index], 'EditMode', true)
            },
            IsEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('IsEditMode: Cant find ' + id);
                    return;
                }
                if (this.$data.Items[index].EditMode) {
                    return true;
                }
                return false;
            },
            //Delete mode
            EnterDeleteMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('EnterDelete: Cant find ' + id);
                    return;
                }
                //Wait .8s before enter delete mode
                this.$set(this.$data.Items[index], 'DeletePromt', true)
            },
            IsDeleteMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('IsDeleteMode: Cant find ' + id);
                    return;
                }
                if (this.$data.Items[index].DeletePromt) {
                    return true;
                }
                return false;
            },
            //Clear new item
            ClearNewItem: function () {
                this.$refs.uploader.Clear();
                this.$data.NewItem.Name = null;
                this.$data.NewItem.DateOfEvent = null;
                this.$data.NewItem.Note = null;
            },
            //Item index stuff
            FindItemIndex: function (id) {
                return this.$data.Items.findIndex(x => x.EventId == id);
            },
            IsDateValid(dateString) {
                return moment(dateString, JSDate, true).isValid();
            },
            DownloadFile: function (id) {
                var s64 = encodeURIComponent(btoa(id));
                var url = API.DownloadAPI;
                url = url.replace('{id}', s64);
                console.log(s64);
                console.log(url);
                window.open(url);
            },
            DownloadZip: function () {
                var s64 = encodeURIComponent(btoa(this.id));
                var url = API.DownloadZipAPI;
                url = url.replace('{id}', s64);
                console.log(s64);
                console.log(url);
                window.open(url);
            },
            //order methods
            OrderByClicked: function (orderBy) {
                if (this.$data.Loading) return;
                this.$data.Loading = true; //prevent click spamming
                //Flip order by
                if (this.$data.OrderBy == orderBy) {
                    this.$data.OrderAsc = !this.$data.OrderAsc;
                }
                else {
                    //Order this column
                    this.$data.OrderBy = orderBy;
                    this.$data.OrderAsc = true;
                }
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            OrderState: function (orderBy) {
                //console.log(orderBy);
                if (orderBy == this.$data.OrderBy) {
                    if (this.$data.OrderAsc)
                        return '&utrif;';
                    return '&dtrif;';
                }
                return '';
            },
            Refresh: function () {
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            PageNavClicked: function (page) {
                if (this.$data.Loading) return;
                this.$data.Loading = true;
                this.$data.OnPage = page;
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            UpdatePagination: function (totalPages, totalRows) {
                this.$data.TotalPages = totalPages;
                this.$data.TotalRows = totalRows;
            },
        }
    }
</script>
<style scoped>
    table.borderless td {
        border: none;
    }
    .attention-border {
        border-color: #ffc107;
    }
    .green-border {
        border-color: #28a745;
    }
    .wrap-text{
        word-wrap: break-word;
    }
    .top-border {
        border-top: 1px solid #dee2e6 !important;
    }
    .no-bottom-margin{
        margin-bottom: 0;
    }
    .shadow-nohover {
        box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
    }
</style>