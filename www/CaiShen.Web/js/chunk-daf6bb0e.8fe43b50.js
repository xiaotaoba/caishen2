(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-daf6bb0e"],{"07b5":function(t,a,s){"use strict";var n=s("57f4"),i=s.n(n);i.a},"1f2c":function(t,a,s){t.exports=s.p+"img/portrait.51118fcd.png"},"1f2f":function(t,a,s){"use strict";s.r(a);var n=function(){var t=this,a=t.$createElement,n=t._self._c||a;return n("div",{staticClass:"account caishen"},[t._m(0),n("div",{staticClass:"yuanbao"},[n("p",{staticClass:"title"},[t._v("总元宝：")]),n("p",{staticClass:"total"},[t._v("1680.00")]),n("div",{staticClass:"yuanbao-wrap"},[n("div",{staticClass:"yuanbao-item"},[n("div",{staticClass:"subTitle"},[n("span",[t._v("可用元宝")]),n("van-icon",{attrs:{name:"play"}})],1),n("p",[t._v("100")])]),n("div",{staticClass:"yuanbao-item"},[n("div",{staticClass:"subTitle"},[n("span",[t._v("冻结元宝")]),n("van-icon",{attrs:{name:"play"}})],1),n("p",[t._v("1650")])]),n("div",{staticClass:"yuanbao-item"},[n("div",{staticClass:"subTitle"},[n("span",[t._v("体现元宝")]),n("van-icon",{attrs:{name:"play"}})],1),n("p",[t._v("20")])])]),n("div",{staticClass:"proportion"},[t._v("元宝与人民币比例为1:1")])]),n("div",{staticClass:"btn-wrap"},[n("van-button",{attrs:{type:"danger"}},[t._v("充值")]),n("van-button",{staticClass:"fr",attrs:{type:"info"}},[t._v("提现")]),n("p",{staticClass:"notice"},[t._v("注意：充值提现金额为100的整数倍，冻结元宝不能提现。解冻规则请关注官方公告。")])],1),n("div",{staticClass:"list"},[n("p",[t._v("账单记录")]),t._l(t.list,function(a,i){return n("div",{key:i,staticClass:"list-item"},[n("img",{attrs:{src:s("feaf")}}),n("div",{staticClass:"item-left"},[n("div",[n("p",[t._v(t._s(a.Thing)+" ")]),n("p",[t._v(t._s(a.Time))])]),n("p",{class:1==a.Type?"num red":"num"},[t._v(t._s(1==a.Type?"+":"-")+t._s(a.Amount))])])])}),t.list.length<1?n("div",{staticClass:"nothing"},[t._v("暂无记录。")]):t._e()],2),n("left-menu")],1)},i=[function(){var t=this,a=t.$createElement,n=t._self._c||a;return n("div",{staticClass:"user"},[n("img",{attrs:{src:s("1f2c")}}),n("p",{staticClass:"ml10"},[t._v("昵称：林深见鹿")])])}],e=s("4bdd"),o=s("eb0e"),c=s("a60a"),u=s.n(c),l={data:function(){return{username:"昵称",photo:"/img/pic_logo_m.png",totalAmount:0,amount:0,lockAmount:0,txAmount:0,list:[]}},components:{TabBar:e["a"],LeftMenu:o["a"]},methods:{getUserInfo:function(){u.a.ajax({type:"GET",url:"/Mobile/Api/Info",data:{},dataType:"json",success:function(t){0==t.status?(this.username=t.U_NickName,this.photo=t.U_Thumbnail,this.amount=t.U_Amount,this.lockAmount=t.U_LockAmount,this.totalAmount=t.U_Amount+t.U_LockAmount,this.txAmount=0):alert(t.msg)}})},getAmountList:function(){u.a.ajax({type:"GET",url:"/Mobile/Api/AmountList",data:{page:1},dataType:"json",success:function(t){0==t.status?this.list=t.data:alert(t.msg)}})}}},p=l;l.getAmountList();var r=p,m=(s("07b5"),s("f69b"),s("17cc")),v=Object(m["a"])(r,n,i,!1,null,"b018fca4",null);a["default"]=v.exports},"44b7":function(t,a,s){},"57f4":function(t,a,s){},f69b:function(t,a,s){"use strict";var n=s("44b7"),i=s.n(n);i.a},feaf:function(t,a,s){t.exports=s.p+"img/caifu-icon.9feb2198.png"}}]);
//# sourceMappingURL=chunk-daf6bb0e.8fe43b50.js.map