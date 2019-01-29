<template>
  <v-container>
    <h1>Cash Withdrawals</h1>
    <v-window v-model="withdrawalWindow">
      <v-window-item>
        <all-withdrawals @addNew="newWithdrawal"></all-withdrawals>
      </v-window-item>
      <v-window-item>
        <new-withdrawal @done="allWithdrawals"></new-withdrawal>
      </v-window-item>
    </v-window>
  </v-container>
</template>

<script>
import AllWithdrawals from '@/components/AllWithdrawals.vue'
import NewWithdrawal from '@/components/NewWithdrawal.vue'

export default {
  components: {
    AllWithdrawals,
    NewWithdrawal
  },
  data() {
    return {
      withdrawalWindow: 0
    }
  },
  methods: {
    newWithdrawal() {
      this.withdrawalWindow = 1
    },
    allWithdrawals() {
      this.withdrawalWindow = 0
    }
  }
}
</script>

<style scoped>
v-container {
  background: cornflowerblue;
}
</style>
