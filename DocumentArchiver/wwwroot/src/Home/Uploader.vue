<template id="uploader">
    <div v-if="File == null">
        <label class="btn btn-link no-bottom-pad">
            <span class="fas fa-cloud-upload-alt" />
            {{buttonText}}<input v-on:change="FileChanged"
                                 type="file"
                                 v-bind:accept="accept"
                                 hidden />
        </label>
    </div>
    <div v-else>
        <span v-bind:class="{'text-danger' : !IsFileValid, 'text-success' : IsFileValid}">{{FormatedFileName}}</span>
        <button v-on:click="Clear" 
                class="btn btn-sm">
            <span class="fas fa-times"></span>
        </button>
    </div>
    
</template>
<script>
    export default {
        name: 'uploader',
        template: '#uploader',
        props: ['accept', 'maxSize', 'buttonText'],
        computed: {
            FirstError: function () {
                return this.$data.Errors[0];
            },
            HasError: function () {
                if (this.$data.Errors.length > 0)
                    return true;
                return false;
            },
            IsFileValid: function () {
                return this.$data.Valid;
            },
            GetFile: function () {
                return this.$data.File;
            },
            FormatedFileName: function () {
                if (!this.$data.File)
                    return '';
                if (this.$data.File.name.length > 20)
                    return this.$data.File.name.substring(0, 17) + '...';
                return this.$data.File.name;
            }
        },
        data: function () {
            return {
                Valid: false,
                File: null,
                Errors: []
            };
        },
        methods: {
            Clear: function () {
                this.$data.File = null;
                this.$data.Valid = false;
                this.$data.Errors = [];
            },
            FileChanged: function (e) {
                var files = e.target.files || e.dataTransfer.files;
                var file = files[0];

                this.$data.File = file;
                this.$data.Valid = false;
                //Check file type
                if (this.IsFileTypeValid(file.name) && this.maxSize >= file.size) {
                    this.$data.Valid = true;
                    return;
                }
                else {
                    this.$data.Errors.push('File upload không hợp lệ');
                    return;
                }
            },
            IsFileTypeValid: function (name) {
                //If no accept is define then true
                var fileName = name.trim().toLowerCase();
                if (!this.accept) return true;
                var split = this.accept.split(',');
                var i = split.length;
                while (i--) {
                    //console.log(fileType + '|' +split[i].trim().toLowerCase());
                    if (fileName.indexOf(split[i].trim().toLowerCase()) !== -1) {
                        return true;
                    }
                }
                return false;
            },
        }
    }
</script>
<style scoped>
    /*For better fit button*/
    .no-bottom-pad {
        padding-bottom: 0;
    }
</style>