<template>
  <div>
    <v-window touchless v-model="cashWithdrawalWindow">
      <v-window-item>
        <edit-cash-withdrawal edit @done="returnToRecent" />
      </v-window-item>
      <v-window-item>
        <recent-cash-withdrawals
          @addCashWithdrawal="addCashWithdrawal"
          @editCashWithdrawal="editCashWithdrawal()"
        ></recent-cash-withdrawals>
      </v-window-item>
      <v-window-item>
        <edit-cash-withdrawal @done="returnToRecent" />
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import RecentCashWithdrawals from '@/components/cashWithdrawal/RecentCashWithdrawals.vue'
import EditCashWithdrawal from '@/components/cashWithdrawal/EditCashWithdrawal.vue'

export default {
  components: {
    RecentCashWithdrawals,
    EditCashWithdrawal
  },
  data() {
    return {
      cashWithdrawalWindow: 1,
      cashWithdrawalToEdit: {}
    }
  },
  methods: {
    addCashWithdrawal() {
      this.cashWithdrawalWindow = 2
      this.scrollToTop()
    },
    editCashWithdrawal() {
      this.cashWithdrawalWindow = 0
      this.scrollToTop()
    },
    returnToRecent() {
      this.cashWithdrawalWindow = 1
      this.scrollToTop()
    },
    scrollToTop() {
      this.$vuetify.goTo(0, {
        duration: 300,
        offset: 0,
        easing: 'easeInOutCubic'
      })
    }
  }
}
</script>

<style scoped></style>
