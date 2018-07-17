USE [ALMS]
GO
/****** Object:  Trigger [dbo].[TR01A_TRG]    Script Date: 2018/7/9 �U�� 08:07:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[TR01A_TRG] ON [dbo].[TR01A]
AFTER INSERT,DELETE,UPDATE
AS
BEGIN
SET NOCOUNT ON;
	declare @err_msg varchar(100);

	if (Select Count(*) From inserted) > 0 and (Select Count(*) From deleted) = 0
	begin
		if exists
		(
			select A.VOU_NO from TR01A as A with(nolock)
			inner join inserted as B with(nolock)
			On A.VOU_NO = b.VOU_NO
			group by A.VOU_NO
			Having COUNT(1)>1
		)
		set @err_msg = '�ǲ����X���i����'
	end

	--if (Select Count(*) From inserted) = 0 and (Select Count(*) From deleted) > 0
	--begin
	--	print ('�R��')
	--end

	if (Select Count(*) From inserted) > 0 and (Select Count(*) From deleted) > 0
	begin
			if exists -- �קK�_�Y
		    (        
		    select I.VOU_NO
		    from Inserted I
		    left join Deleted D on I.TR01A_ID=D.TR01A_ID
		    where (I.VOU_NO<>D.VOU_NO)
		    )
		set @err_msg = '�ǲ����X���i�ק�'
	end
	if @err_msg <> ''
	begin
		set @err_msg = 'utg : (TR01A) '+ @err_msg  
		raiserror (@err_msg, 16, 1)  
		rollback
	end
END
