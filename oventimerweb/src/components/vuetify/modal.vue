<template>
    <v-row justify="center">
        <v-dialog @click:outside="clickOutside" hide-overlay v-model="show" :persistent="isPersistent" max-width="900px">
            <v-card>
                <v-card-title class="no-padding">
                    <v-row>
                        <v-col cols="10">
                            <span class="subtitle-2">{{ title }}</span>
                        </v-col>
                        <v-col cols="2" class="text-right">
                            <v-icon @click="close">mdi-close-circle</v-icon>
                        </v-col>
                    </v-row>                    
                </v-card-title>

                <v-card-text>                                         
                    <slot name="content"></slot>                    
                </v-card-text>

                <v-card-actions>
                    <div class="flex-grow-1"></div>
                    <slot name="actions"></slot>                    
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-row>
</template>

<script>
export default {
    props: {
        persistent: Boolean,
        title: String
    },
    data() {
        return {
            isPersistent: !!this.persistent,
            show: true
        };
    },
    methods: {
        clickOutside: function() {                        
            // The click outside event handler ignores the persistent flag. Make sure it's not persistent.
            if(!this.isPersistent) {
                this.close();
            }
        },
        close: function() {
            this.$emit('close');
        }
    }
}
</script>

<style lang="scss" scoped>
    .no-padding {
        padding-top: 0px !important;        
    }
</style>