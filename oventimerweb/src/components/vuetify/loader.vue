<template>
  <div class="text-center">
    <v-dialog v-model="show" hide-overlay persistent width="300">
      <v-card color="primary" dark>
        <v-card-text>
          {{ $isNullOrWhitespace(text) ? 'Loading...' : text }}
          
          <v-progress-linear
            indeterminate
            color="white"
            class="mb-0"
          ></v-progress-linear>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
    props: {
        text: String
    },
    data() {
      return {
        show: true
      };
    }
}
</script>