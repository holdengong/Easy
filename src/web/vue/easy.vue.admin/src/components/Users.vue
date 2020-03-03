<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{path:'/'}">首页</el-breadcrumb-item>
      <el-breadcrumb-item>用户列表</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card>
      <el-row :gutter="20" style="margin-bottom:20px">
        <el-col :span="7">
          <el-input placeholder="请输入内容" v-model="queryInfo.keywords">
            <el-button slot="append" icon="el-icon-search" @click="getUsers"></el-button>
          </el-input>
        </el-col>
        <el-col :span="4">
          <el-button type="primary" @click="addDialogFormVisible=true">添加用户</el-button>
        </el-col>
      </el-row>

      <el-table :data="userlist">
        <el-table-column label="#" type="index"></el-table-column>
        <el-table-column label="姓名" prop="userName"></el-table-column>
        <el-table-column label="邮箱" prop="email"></el-table-column>
        <el-table-column label="电话" prop="mobile"></el-table-column>
        <el-table-column label="角色" prop="role"></el-table-column>
        <el-table-column label="状态">
          <!-- 作用域插槽 -->
          <template slot-scope="scope">
            <el-switch
              v-model="scope.row.isActive"
              @change="updateUserState(scope.row.id,scope.row.isActive)"
            ></el-switch>
          </template>
        </el-table-column>
        <el-table-column label="操作" prop="role">
          <template slot-scope="scope">
            <el-button
              type="primary"
              icon="el-icon-edit"
              size="mini"
              @click="openModifyDialog(scope.row.id)"
            ></el-button>
            <el-button
              type="danger"
              icon="el-icon-delete"
              size="mini"
              @click="deleteUser(scope.row.id)"
            ></el-button>
            <el-tooltip effect="dark" content="分配角色" placement="top" :enterable="false">
              <el-button type="warning" icon="el-icon-setting" size="mini"></el-button>
            </el-tooltip>
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

    <el-dialog title="添加用户" :visible.sync="addDialogFormVisible" @close="clearFields">
      <el-form :model="form" :rules="formRules" ref="formRef" label-width="70px">
        <el-form-item label="用户名" prop="userName">
          <el-input v-model="form.userName"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="form.password"></el-input>
        </el-form-item>
        <el-form-item label="手机" prop="mobile">
          <el-input v-model="form.mobile"></el-input>
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="form.email"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="addDialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="addUser">确 定</el-button>
      </div>
    </el-dialog>

    <el-dialog title="修改用户" :visible.sync="modifyDialogFormVisible" @close="clearFields">
      <el-form :model="form" :rules="formRules" ref="formRef" label-width="70px">
        <el-form-item label="用户名" prop="userName">
          <el-input v-model="form.userName" disabled></el-input>
        </el-form-item>
        <el-form-item label="手机" prop="mobile">
          <el-input v-model="form.mobile"></el-input>
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="form.email"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="modifyDialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="modifyUser">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
export default {
  created() {
    this.getUsers();
  },
  data() {
    var checkEmail = (rule, value, cb) => {
      const regEmail = /^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
      if (regEmail.test(value)) {
        return cb();
      }
      cb(new Error("请输入合法邮箱"));
    };

    var checkMobile = (rule, value, cb) => {
      const regMobile = /^1[3456789]\d{9}$/;
      if (regMobile.test(value)) {
        return cb();
      }
      cb(new Error("请输入合法手机号"));
    };

    return {
      userlist: [],
      queryInfo: {
        pageIndex: 1,
        pageSize: 10,
        keywords: ""
      },
      form: {
        id: "",
        userName: "",
        password: "",
        mobile: "",
        email: ""
      },
      formRules: {
        userName: [
          { required: true, message: "请输入用户名", trigger: "blur" },
          {
            min: 3,
            max: 36,
            message: "用户名长度在3~36个字符之间",
            trigger: "blur"
          }
        ],
        password: [
          { required: true, message: "请输入密码", trigger: "blur" },
          { min: 4, message: "密码长度不能少于4个字符", trigger: "blur" }
        ],
        mobile: [
          { required: true, message: "请输入手机号", trigger: "blur" },
          { validator: checkMobile, trigger: "blur" }
        ],
        email: [
          { required: true, message: "请输入用户名", trigger: "blur" },
          { validator: checkEmail, trigger: "blur" }
        ]
      },
      total: 0,
      addDialogFormVisible: false,
      modifyDialogFormVisible: false
    };
  },
  methods: {
    async getUsers() {
      const { data: result } = await this.$http.get("users", {
        params: this.queryInfo
      });
      if (result.code != 0) {
        return this.$message.error("用户列表查询失败");
      }
      console.log(result);
      this.userlist = result.data.list;
      this.total = result.data.total;
    },
    handleSizeChange(val) {
      this.queryInfo.pageSize = val;
      this.getUsers();
    },
    handleCurrentChange(val) {
      this.queryInfo.pageIndex = val;
      this.getUsers();
    },
    clearFields() {
      this.$refs.formRef.resetFields();
    },
    addUser() {
      this.$refs.formRef.validate(async valid => {
        if (!valid) return;
        const { data: result } = await this.$http.post("users", this.form);
        if (result.code != 0) {
          this.$message.error(result.message);
        }
        this.$message.success("添加用户成功");
        this.addDialogFormVisible = false;
        this.getUsers();
      });
    },
    deleteUser(id) {
      this.$confirm("此操作将永久删除用户, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(async () => {
          const { data: result } = await this.$http.delete(`user/${id}`);
          if (result.code == 0) {
            this.$message.success("删除成功");
            this.getUsers();
          } else {
            this.$message.error(result.message);
          }
        })
        .catch();
    },
    async updateUserState(id, isActive) {
      const { data: result } = await this.$http.put(`user/${id}/state`, {
        isActive: isActive
      });
      if (result.code == 0) {
        this.$message.success("修改成功");
      }
    },
    async openModifyDialog(id) {
      const { data: result } = await this.$http.get(`user/${id}`);

      console.log(result)

      this.form.id = result.data.id
      this.form.userName = result.data.userName
      this.form.mobile = result.data.mobile
      this.form.email = result.data.email

      this.modifyDialogFormVisible = true;
    },
    async modifyUser() {
      const { data: result } = await this.$http.put(`user/${this.form.id}`, this.form)
      if (result.code == 0) {
        this.modifyDialogFormVisible = false
        this.$message.success("修改成功")
        this.getUsers()
      }else{
        this.$message.error(result.message)
      }
    }
  }
};
</script>

<style lang="less" scoped>
.el-pagination {
  margin-top: 15px;
}
</style>