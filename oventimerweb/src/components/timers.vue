<template>
    <div>
        <v-row justify="center">
            <v-col cols="2"><v-text-field class="display-1" :error="!hourValid" label="Number of Timers" placeholder="10" outlined :value="count" @change="count = $event"></v-text-field></v-col>
            <v-col cols="2"><v-text-field class="display-1" :error="!hourValid" label="Default Hours" placeholder="00" outlined :value="hour" @change="hour = $event"></v-text-field></v-col>
            <span class="display-3">:</span>
            <v-col cols="2"><v-text-field class="display-1" :error="!minuteValid" label="Default Minutes" placeholder="00" outlined :value="minute" @change="minute = $event"></v-text-field></v-col>
            <span class="display-3">:</span>
            <v-col cols="2"><v-text-field class="display-1" :error="!secondValid" label="Default Seconds" placeholder="00" outlined :value="second" @change="second = $event"></v-text-field></v-col>
            <v-col cols="3">
                <v-btn class="mt-2" :disabled="!timeValid" large color="primary" @click="setDefaultTime">Set</v-btn>
                <v-btn class="ml-2 mt-2" :disabled="!timeValid" large color="error" @click="setDefaultTime">Reset All</v-btn>
            </v-col>            
        </v-row>

        <v-row justify="center" no-gutters v-if="showClocks">
            <v-col :ref="'items'" cols="3" justify="center" v-bind:key="item.id" v-for="item of items">
                <timer v-model="item.time" :ref="'item-' + item.id" :caption="item.id"></timer>                
            </v-col>
        </v-row>
    </div>
</template>

<script>
import { HubConnectionBuilder, LogLevel, HttpTransportType } from '@aspnet/signalr';
import Timer from './timer';

export default {
    components: {
        timer: Timer
    },  
    created() {
        this.loadTimers();
    },
    mounted() {      
        this.listenForTimers();
    },
    data() {
        return {
            count: 10,
            hour: '00',
            items: [],
            minute: '30',
            second: '00',            
            showClocks: false
        };
    },
    computed: {
        hourValid: function() {
            return this.isValid(this.hour);
        },
        minuteValid: function() {
            return this.isValid(this.minute);            
        },
        secondValid: function() {
            return this.isValid(this.second);            
        },
        timeValid: function() {
            return this.hourValid && this.minuteValid && this.secondValid;
        }
    },
    watch: {
        hour: function(val) {            
            if(val.length < 2) {
                this.hour = this.$getFormatTime(val);
            }            
        },
        minute: function(val) {
            if(val.length < 2) {
                this.minute = this.$getFormatTime(val);
            }
        },
        second: function(val) {
            if(val.length < 2) {
                this.second = this.$getFormatTime(val);
            }
        }
    },
    methods: {
        createItems: async function() {            
            this.items = [];
            this.showClocks = false;

            for (let i = 1; i < Number.parseInt(this.count) + 1; i++) {                
                let item = {
                    id: i.toString(),
                    hour: this.hour,
                    minute: this.minute,
                    second: this.second
                };

                item.time = this.getTimeValue(item);

                this.items.push(item);
            }

            let timeSetViewModel = {
                defaults: {
                    defaultTimeSet: {
                        hour: this.hour,
                        minute: this.minute,
                        second: this.second
                    },
                    timers: Number.parseInt(this.count)
                },
                timesets: this.items
            }

            await this.$http.post('timer', timeSetViewModel).catch(err => {
                this.$showError(err, 'Set Timers');
            });

            this.showClocks = true;
        },
        getTimeValue: function(item) {
            return `${item.hour}:${item.minute}:${item.second}`;
        },
        isValid: function(time) {
            return !this.$isNullOrWhitespace(time) && !isNaN(time);
        },
        listenForTimers: function() {
            const connection = new HubConnectionBuilder()
                .withUrl('timerHub', { transport: HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling })
                .configureLogging(LogLevel.Information)
                .build();    
            
            let _self = this;
            connection.start().then(() => {                
                connection.on('ReceiveInput', (data) => {                    
                    let key = 'item-' + data.trim();                    
                    
                    if(_self.$refs[key] != null && _self.$refs[key].length) {                        
                        _self.$refs[key][0].toggle();
                    }
                }); 

                connection.invoke("StartListener").catch(function(err) {
                    _self.$showError(err, 'Timer Listener');
                });
            }).catch((err) => {      
                _self.$showError(err, 'Timer Service Connection');
            });
        },
        loadTimers: async function() {
            await this.$http.get('timer').then(res => {        
                if(res.data) {        
                    if(res.data.defaults) {
                        let defaults = res.data.defaults;

                        if(defaults.defaultTimeSet) {
                            let defaultSet = defaults.defaultTimeSet;
                            
                            this.hour = defaultSet.hour;
                            this.minute = defaultSet.minute;
                            this.second = defaultSet.second;
                        }

                        if(defaults.timers) {
                            this.count = Number.parseInt(defaults.timers);
                        }
                    }            
                    
                    if(res.data.timeSets && res.data.timeSets.length) {                        
                        this.items = res.data.timeSets;                    

                        for(let item of this.items) {
                            item.time = this.getTimeValue(item);
                        }

                        this.showClocks = true;
                    }                    
                }
            }).catch(err => {
                this.$showError(err, 'Get Timers');
            });
        },
        setDefaultTime: function() {
            if(this.timeValid) {
                this.createItems();
            }
        }
    }
}
</script>

<style lang="scss" scoped>
</style>