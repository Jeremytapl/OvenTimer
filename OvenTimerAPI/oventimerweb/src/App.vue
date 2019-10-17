<template>
  <v-app id="app">
    <v-app-bar app clipped-left>      
      <v-toolbar-title><span @click="$router.push('/')" style="cursor: pointer;">Timer</span></v-toolbar-title>
    </v-app-bar>

    <v-content>
      <v-container fluid>
        <!-- <modal :show="showError" :title="errorTitle" @close="showError = false" v-if="showError">
          <template v-slot:content>
            <v-alert type="error" >
              {{ errorText }}
            </v-alert>
          </template>
        </modal>  --> 
        <v-row justify="center" v-if="showError">
          <v-alert dismissible type="error" >
              {{ errorText }}
            </v-alert>
        </v-row>

        <router-view />
      </v-container>
    </v-content>
  </v-app>
</template>

<script>
//import Modal from './components/vuetify/modal';

export default {
  components: {
    //modal: Modal
  },
  created() {
    document.title = "Timer";
    
    this.$vuetify.theme.dark = true;
    this.$root.$on('error', this.showErrorAlert);
  },
  data() {
    return {
      errorText: '',
      errorTitle: '',
      showError: false
    };
  },
  watch: {
    '$route' (to) {
      document.title = to != null ? 'Timer - ' + to.name : 'Timer';
    },
    showError: function(val) {
      // If error is turned off, reset text
      if(!val) {
        this.errorText = '';
      }      
    }
  },
  methods: {
    showErrorAlert: function(errorText, errorTitle) {            
      this.errorText = errorText;
      this.errorTitle = this.$isNullOrWhitespace(errorTitle) ? 'Error' : 'Error - ' + errorTitle;
      
      this.showError = true;
    }
  }
}
</script>