<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{path:'/'}">首页</el-breadcrumb-item>
      <el-breadcrumb-item>角色列表</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card>
      <el-row :gutter="20" style="margin-bottom:20px">
        <el-col :span="7">
          <el-input placeholder="请输入内容" v-model="queryInfo.keywords">
            <el-button slot="append" icon="el-icon-search" @click="getList"></el-button>
          </el-input>
        </el-col>
        <el-col :span="4">
          <el-button type="primary" @click="addDialogFormVisible=true">添加角色</el-button>
        </el-col>
      </el-row>

      <el-table :data="list">
        <el-table-column label="#" type="index"></el-table-column>
        <el-table-column label="名称" prop="name"></el-table-column>
        <el-table-column label="操作" prop="role">
          <template slot-scope="scope">
            <el-button
              type="primary"
              icon="el-icon-edit"
              size="mini"
              @click="openModify(scope.row.id)"
            ></el-button>
            <el-button
              type="danger"
              icon="el-icon-delete"
              size="mini"
              @click="remove(scope.row.id)"
            ></el-button>
          </template>
        </el-table-column>
      </el-table>

      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="queryInfo.pageIndex"
        :page-sizes="[10, 20, 50, 100]"
        :page-size="10"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      ></el-pagination>
    </el-card>

    <el-dialog title="添加角色" :visible.sync="addDialogFormVisible" @close="clearAddFields">
      <el-form :model="addForm" :rules="formRules" ref="addFormRef" label-width="70px">
        <el-form-item label="角色名" prop="name">
          <el-input v-model="addForm.name"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="addDialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="add">确 定</el-button>
      </div>
    </el-dialog>

    <el-dialog title="修改角色" :visible.sync="modifyDialogFormVisible" @close="clearModifyFields">
      <el-form :model="modifyForm" :rules="formRules" ref="modifyFormRef" label-width="70px">
        <el-form-item label="角色名" prop="name">
          <el-input v-model="modifyForm.name"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="modifyDialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="modify">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
export default {
  created() {
    this.getList();
  },
  data() {
    return {
      list: [],
      queryInfo: {
        pageIndex: 1,
        pageSize: 10,
        keywords: ""
      },
      addForm: {
        name: ""
      },
      modifyForm: {},
      formRules: {
        name: [{ required: true, message: "请输入名称", trigger: "blur" }]
      },
      total: 0,
      addDialogFormVisible: false,
      modifyDialogFormVisible: false
    };
  },
  methods: {
    async getList() {
      const { data: result } = await this.$http.get("roles", {
        params: this.queryInfo
      });
      if (result.code != 0) {
        return this.$message.error("服务异常");
      }
      this.list = result.data.list;
      this.total = result.data.total;
    },
    handleSizeChange(val) {
      this.queryInfo.pageSize = val;
      this.getList();
    },
    handleCurrentChange(val) {
      this.queryInfo.pageIndex = val;
      this.getList();
    },
    clearAddFields() {
      this.$refs.addFormRef.resetFields();
    },
    clearModifyFields() {
      this.$refs.modifyFormRef.resetFields();
    },
    add() {
      this.$refs.addFormRef.validate(async valid => {
        if (!valid) return;
        const { data: result } = await this.$http.post("roles", this.addForm);
        if (result.code != 0) {
          this.$message.error(result.message);
        }
        this.$message.success("操作成功");
        this.addDialogFormVisible = false;
        this.getList();
      });
    },
    remove(id) {
      this.$confirm("此操作将永久删除数据, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(async () => {
          const { data: result } = await this.$http.delete(`role/${id}`);
          if (result.code == 0) {
            this.$message.success("删除成功");
            this.getList();
          } else {
            this.$message.error(result.message);
          }
        })
        .catch();
    },
    async openModify(id) {
      const { data: result } = await this.$http.get(`role/${id}`);
      this.modifyForm = result.data;
      this.modifyDialogFormVisible = true;
    },
    async modify() {
      const { data: result } = await this.$http.put(
        `role/${this.modifyForm.id}`,
        this.modifyForm
      );
      if (result.code == 0) {
        this.modifyDialogFormVisible = false;
        this.$message.success("操作成功");
        this.getList();
      } else {
        this.$message.error(result.message);
      }
    }
  }
};
</script>

<style lang="less" scoped>
</style>