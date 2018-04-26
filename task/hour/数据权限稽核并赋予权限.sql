INSERT INTO sys_user_datapurview(user_id,node_id)
(
	SELECT t1.id,t2.org_id FROM 
	(
		SELECT id,name FROM sys_user WHERE id IN
		(
		1133,
		822	,
		387	,
		402	,
		2453,
		1548
		)
	) t1
	OUTER APPLY
	(
		SELECT a_a.id AS org_id,a_a.title,b_b.user_id,b_b.node_id FROM 
		(SELECT * FROM sys_organize_node ta WHERE ta.node_code = 101 AND ta.pid = 398 AND use_status = 1)a_a
		LEFT JOIN [sys_user_datapurview] b_b
		ON a_a.id=b_b.node_id
		AND b_b.user_id = t1.id

	) t2
	WHERE t2.node_id IS NULL
)